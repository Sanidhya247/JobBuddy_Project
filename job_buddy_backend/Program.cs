using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
// using EntityFrameworkCore.UseRowNumberForPaging;
using FluentValidation.AspNetCore;
using job_buddy_backend.Core;
using job_buddy_backend.Core.ChatService;
using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.Core.Interfaces.Chat;
using job_buddy_backend.Core.Interfaces.Payment;
using job_buddy_backend.Core.Interfaces.UserProfile;
using job_buddy_backend.Core.Payment;
using job_buddy_backend.Core.Services;
using job_buddy_backend.Core.UserProfile;
using job_buddy_backend.DTO;
using job_buddy_backend.DTO.Mapping;
using job_buddy_backend.DTO.UserProfile;
using job_buddy_backend.Helpers;
using job_buddy_backend.Models.DataContext;
using job_buddy_backend.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace job_buddy_backend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Set up Serilog for logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();


            if (builder.Environment.IsProduction())
            {
                // Use Managed Identity to authenticate to Azure Key Vault
                string keyVaultName = "JobBuddyKeyVault"; 
                var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

                var secretClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());

                try
                {
                    // Fetch the production connection string from Key Vault
                    KeyVaultSecret secret = await secretClient.GetSecretAsync("ProdConnection");
                    string prodConnectionString = secret.Value;

                    // Store the connection string in configuration
                    builder.Configuration["ConnectionStrings:DefaultConnection"] = prodConnectionString;
                }
                catch (Exception ex)
                {
                    Log.Error("Error fetching secret from Key Vault: {0}", ex.Message);
                    throw;
                }
            }
            // Configure SQL Server
            builder.Services.AddDbContext<JobBuddyDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    // o => o.UseRowNumberForPaging()
                    //sqlOptions => sqlOptions.EnableRetryOnFailure()
                ));
            builder.Services.AddSignalR();

            // Register services
            RegisterCoreServices(builder.Services);

            // Configure JWT authentication
            ConfigureAuthentication(builder.Services, builder.Configuration);

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Register FluentValidation and Automapper
            RegisterValidationsAndMappings(builder.Services);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Job Buddy API", Version = "v1" });

                // Define the security scheme
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 5242880; // Set the maximum file size to 5MB
            });

            // Build the final app
            var app = builder.Build();

            // Run database migrations safely
            await InitializeDatabase(app);

            // Configure HTTP Request Pipeline
            ConfigurePipeline(app);

            app.Run();
        }

        // Configure Core Services
        private static void RegisterCoreServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IEmployerProfileService, EmployerProfileService>();
            services.AddScoped<IResumeService, ResumeService>();
            services.AddHttpClient<LocationService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IJobListingService, JobListingService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IConnectionService, ConnectionService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<AdminService>();
            services.AddScoped<ContactUsService>();
            services.AddScoped<AtsScoringService>();

            services.AddControllers();
        }


        // Configure Authentication and Secure Key Fetching
        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            // Use a placeholder JWT key initially. It will be updated later using the ConfigurationService.
            var jwtKeyPlaceholder = "TemporaryKeyForMigration";
            var jwtIssuer = configuration["JwtSettings:Issuer"];
            var jwtAudience = configuration["JwtSettings:Audience"];

            // Configure JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeyPlaceholder))
                };
            });
        }

        // Register Validators and AutoMapper
        private static void RegisterValidationsAndMappings(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterUserValidator>());
                //.AddJsonOptions(options =>
                //{
                //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                //    options.JsonSerializerOptions.WriteIndented = true;
                //}); 
            services.AddTransient<IValidator<RegisterUserDto>, RegisterUserValidator>();
            services.AddTransient<IValidator<LoginUserDto>, LoginUserValidator>();
            services.AddTransient<IValidator<UpdateUserProfileDto>, UpdateUserProfileValidator>();
            services.AddAutoMapper(typeof(UserProfile));
        }

        // Initialize the Database and Run Migrations
        private static async Task InitializeDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<JobBuddyDbContext>();
                var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();

                try
                {
                    // Apply pending migrations
                    await dbContext.Database.MigrateAsync();

                    // Seed the configuration settings after migration
                    await SeedConfigurationAsync(scope.ServiceProvider, configurationService);
                }
                catch (Exception ex)
                {
                    Log.Error("An error occurred while initializing the database. Error: {0}", ex.Message);
                }
            }
        }

        // Configure HTTP Request Pipeline
        private static void ConfigurePipeline(WebApplication app)
        {

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Job Buddy API V1");
                });

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigins");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }

        // Seed JWT Key and other configuration from the database
        private static async Task SeedConfigurationAsync(IServiceProvider serviceProvider, IConfigurationService configurationService)
        {
            var jwtKey = await configurationService.GetSettingAsync("JwtKey");

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured in the database.");
            }

            // Manually update the token validation parameters after the app starts
            var jwtOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();

            jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme).TokenValidationParameters.IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        }
    }
}

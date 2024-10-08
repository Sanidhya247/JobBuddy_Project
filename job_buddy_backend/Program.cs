using FluentValidation;
using FluentValidation.AspNetCore;
using job_buddy_backend.Core;
using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.DTO.Mapping;
using job_buddy_backend.Helpers;
using job_buddy_backend.Models.DataContext;
using job_buddy_backend.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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

            // Configure SQL Server
            builder.Services.AddDbContext<JobBuddyDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()
                ));

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
            builder.Services.AddSwaggerGen();

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
            services.AddTransient<IValidator<RegisterUserDto>, RegisterUserValidator>();
            services.AddTransient<IValidator<LoginUserDto>, LoginUserValidator>();
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
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigins");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }

        // Seed JWT Key and other configuration from the database
        private static async Task SeedConfigurationAsync(IServiceProvider serviceProvider, IConfigurationService configurationService)
        {
            var jwtKey = await configurationService.GetSettingAsync("JwtKey");
            var jwtOptions = serviceProvider.GetRequiredService<JwtBearerOptions>();

            // Update JWT key from database configuration
            jwtOptions.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        }
    }
}

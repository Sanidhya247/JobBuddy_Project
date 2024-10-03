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

            builder.Logging.ClearProviders(); // Clear default log providers
            builder.Logging.AddSerilog(); //Add the serilog settings to app

            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

            // Configure SQL Server
            builder.Services.AddDbContext<JobBuddyDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register the memory cache DI service
            builder.Services.AddMemoryCache();

            // Register services and interfaces
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

            // Fetch JWT settings using a temporary service provider before building the app
            string jwtKey = "";
            string jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
            string jwtAudience = builder.Configuration["JwtSettings:Audience"];

            var tempServiceProvider = builder.Services.BuildServiceProvider();
            using (var scope = tempServiceProvider.CreateScope())
            {
                var configurationService = scope.ServiceProvider.GetRequiredService<IConfigurationService>();
                jwtKey = await configurationService.GetSettingAsync("JwtKey");
            }

            // Configure JWT Authentication
            builder.Services.AddAuthentication(options =>
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
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],,
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            // Add Controllers and FluentValidation
            builder.Services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterUserValidator>());

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowSpecificOrigins",
                                  policy =>
                                  {
                                      policy.WithOrigins(allowedOrigins)
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });

            // Register Validators and Mappings
            builder.Services.AddTransient<IValidator<RegisterUserDto>, RegisterUserValidator>();
            builder.Services.AddTransient<IValidator<LoginUserDto>, LoginUserValidator>();
            builder.Services.AddAutoMapper(typeof(UserProfile));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Now build the final app
            var app = builder.Build();

            // Configure HTTP Request Pipeline
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

            app.Run();
        }
    }
}

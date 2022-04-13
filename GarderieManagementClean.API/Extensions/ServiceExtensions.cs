using GarderieManagementClean.API.Options;
using GarderieManagementClean.Application.Implementation;
using GarderieManagementClean.Application.Interfaces;
using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Interfaces.Services;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Application.ServicesImplementation;
using GarderieManagementClean.Infrastructure.Identity;

using GarderieManagementClean.Infrastructure.Persistence;
using GarderieManagementClean.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace GarderieManagementClean.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {

            OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                // BearerFormat = "JWT",
                Description = "Specify the authorization token.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                Scheme = "Bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".

            };

            OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference //The name of the previously defined security scheme.
                        {
                            Id = "Bearer", //The name of the previously defined security scheme.
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new string[]{ }
                }
            };

            OpenApiContact contactInfo = new OpenApiContact()
            {
                Name = "Adel Kouaou",
                Email = "adel.kouaou@hotmail.com",
                Url = new System.Uri("https://hopeful-jackson-37a49d.netlify.app/")
            };

            OpenApiInfo info = new OpenApiInfo()
            {
                Title = "Garderie Management API",
                Version = "v1",
                Contact = contactInfo,
                License = null,
            };


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", info);
                options.AddSecurityDefinition("Bearer", securityDefinition);
                options.AddSecurityRequirement(securityRequirements);
            });
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Allow all",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }


        public static void ConfigureApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            // ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {

            //Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();



            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(3);
                
            });




        }

        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            // services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureControllers(this IServiceCollection services)
        {

            services.AddControllers()
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               });
        }

        public static void AddServicesAndRepositories(this IServiceCollection services, IConfiguration configuration)
        {

            //identity service/repository
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();


            //garderie service/repository
            services.AddScoped<IGarderieService, GarderieService>();
            services.AddScoped<IGarderieRepository, GarderieRepository>();

            //email service
            services.AddTransient<IEmailService, EmailService>();

            //TODO: Add Logger




        }
        public static void AddSingletonJwtSettings(this IServiceCollection services, IConfiguration configuration, out JwtSettings jwtSettings)
        {
            jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);
        }

        public static void ConfigureAuthentification(this IServiceCollection services, IConfiguration configuration, JwtSettings jwtSettings)
        {

            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,

                RequireExpirationTime = true,
                ValidateLifetime = true, // In any other application other then demo this needs to be true

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),


                ClockSkew = new TimeSpan(0, 0, 15)
            };

            services.AddSingleton(TokenValidationParameters);

            services
               .AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

               })
               .AddJwtBearer(options =>
               {
                   options.SaveToken = true;
                   options.TokenValidationParameters = TokenValidationParameters;
               });
        }
    }
}

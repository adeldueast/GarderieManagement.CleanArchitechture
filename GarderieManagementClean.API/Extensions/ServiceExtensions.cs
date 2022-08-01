using B2Net.Models;
using GarderieManagementClean.API.Options;
using GarderieManagementClean.Application.Implementation;
using GarderieManagementClean.Application.Interfaces;
using GarderieManagementClean.Application.Interfaces.Repositories;
using GarderieManagementClean.Application.Interfaces.Services;
using GarderieManagementClean.Application.Models;
using GarderieManagementClean.Application.ServicesImplementation;
using GarderieManagementClean.Domain.Entities;
using GarderieManagementClean.Infrastructure.Identity;

using GarderieManagementClean.Infrastructure.Persistence;
using GarderieManagementClean.Infrastructure.Repositories;
using GarderieManagementClean.Infrastructure.Repositories.AttendanceRepository;
using GarderieManagementClean.Infrastructure.Repositories.EnfantRepository;
using GarderieManagementClean.Infrastructure.Repositories.GroupRepository;
using GarderieManagementClean.Infrastructure.Repositories.JournalRepository;
using GarderieManagementClean.Infrastructure.Repositories.NotificationRepository;
using GarderieManagementClean.Infrastructure.Repositories.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Threading.Tasks;

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
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });
        }


        public static void ConfigureApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            // ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {

                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                string connStr;
                if (env == "Development")
                {
                    connStr = config.GetConnectionString(@"DefaultConnectionDev");

                }
                else if (env == "Production")
                {
                    connStr = config.GetConnectionString("DefaultConnectionProd");

                }
                else
                {
                    connStr = config.GetConnectionString(@"DefaultConnectionDev");
                }

                options.UseLazyLoadingProxies();
                options.UseNpgsql(connStr);


            });

        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            //services.AddTransient<IUserValidator<ApplicationUser>, OptionalEmailUserValidator<MonotypeIdentityUser>>();
            services.AddTransient<IUserValidator<ApplicationUser>, OptionalEmailUserValidator<ApplicationUser>>();

            //Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.User.RequireUniqueEmail = true;


                //options.SignIn.RequireConfirmedEmail = true;

            })
            //.AddUserValidator<OptionalEmailUserValidator<ApplicationUser>>()
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


            //group  service/repository
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupRepository, GroupRepository>();

            //enfant  service/repository
            services.AddScoped<IEnfantService, EnfantService>();
            services.AddScoped<IEnfantRepository, EnfantRepository>();


            //users  service/repository
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            //users  service/repository
            services.AddScoped<IJournalDeBordService, JournalDeBordService>();
            services.AddScoped<IJournalDeBordRepository, JournalRepository>();


            //users  service/repository
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            //enfant  service/repository
            //services.AddScoped<IAttendanceService, AttendanceService>();
            //services.AddScoped<IAttendanceRepository, AttendanceRepository>();

            //email service
            services.AddTransient<IEmailService, EmailService>();






        }
        public static void AddSingletonJwtSettings(this IServiceCollection services, IConfiguration configuration, out JwtSettings jwtSettings)
        {
            jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);


        }


        public static void AddBackBlazeClientSingleton(this IServiceCollection services, IConfiguration configuration)
        {
            var B2Options = new B2Options();
            configuration.Bind(nameof(B2Options), B2Options);
            services.AddSingleton(B2Options);
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








                   // Configure the Authority to the expected value for
                   // the authentication provider. This ensures the token
                   // is appropriately validated.
                   // options.Authority = "Authority URL"; // TODO: Update URL

                   // We have to hook the OnMessageReceived event in order to
                   // allow the JWT authentication handler to read the access
                   // token from the query string when a WebSocket or 
                   // Server-Sent Events request comes in.

                   // Sending the access token in the query string is required due to
                   // a limitation in Browser APIs. We restrict it to only calls to the
                   // SignalR hub in this code.
                   // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
                   // for more information about security considerations when using
                   // the query string to transmit the access token.
                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var accessToken = context.Request.Query["access_token"];

                           // If the request is for our hub...
                           var path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken))//&& (path.StartsWithSegments("/hubs/chat")
                           {
                               // Read the token out of the query string
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };






               });
        }
    }
}

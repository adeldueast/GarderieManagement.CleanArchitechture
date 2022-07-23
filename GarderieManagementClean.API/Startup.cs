using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.API.HubConfig;
using GarderieManagementClean.API.Options;
using GarderieManagementClean.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarderieManagementClean.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Registering SignalR
            services.AddSignalR(options =>
            options.EnableDetailedErrors = true
            );

            //Cors
            services.ConfigureCors();


            //ApplicationDbContext
            services.ConfigureApplicationDbContext(Configuration);


            //Identity
            services.ConfigureIdentity();

            //UnitOfWork Singleton
            //services.ConfigureUnitOfWork();

            //Controllers
            services.ConfigureControllers();

            //JwtSettings Singleton
            services.AddSingletonJwtSettings(Configuration, out JwtSettings jwtSettings);

            //JWT Authentification configuration (Jwt Bearer)
            services.ConfigureAuthentification(Configuration, jwtSettings);

            //Swagger configuration
            services.ConfigureSwagger();

            //AddScoped Services
            services.AddServicesAndRepositories(Configuration);


            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();


            //AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseCors(o =>
            //{
            //    o.AllowAnyHeader();
            //    o.AllowAnyMethod();
            //    o.AllowAnyOrigin();
            //});



            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();



            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(options => { options.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(options => options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChildrenHub>("/Children");
            });
        }
    }
}

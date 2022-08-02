using GarderieManagementClean.API.Extensions;
using GarderieManagementClean.API.HubConfig;
using GarderieManagementClean.API.Options;
using GarderieManagementClean.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

namespace GarderieManagementClean.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
         ;
        }

        public IConfiguration Configuration { get; }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Cors
            services.ConfigureCors();

            // Registering SignalR
            services.AddSignalR(options =>
            options.EnableDetailedErrors = true
            );

         


            //ApplicationDbContext
            services.ConfigureApplicationDbContext(Configuration);


            //Identity
            services.ConfigureIdentity();

            //UnitOfWork Singleton
            //services.ConfigureUnitOfWork();

            //Controllers
            services.ConfigureControllers();

            //black balze client
            services.ConfigureSecretSingletons(Configuration, out JwtSettings jwtSettings);
           

            // Add services to the container.
            services.AddHttpClient();

            //JwtSettings Singleton
           // services.AddSingletonJwtSettings(Configuration );

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

                var swaggerOptions = new SwaggerOptions();
                Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
                app.UseSwagger(options => { options.RouteTemplate = swaggerOptions.JsonRoute; });
                app.UseSwaggerUI(options => options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
       
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChildrenHub>("/Children");
            });
        }
    }
}

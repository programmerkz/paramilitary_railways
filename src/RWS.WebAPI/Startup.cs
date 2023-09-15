using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RWS.Application.Extensions;
using RWS.Application.Interfaces;
using RWS.Infrastructure.Authentification.Extensions;
using RWS.Infrastructure.Persistence.Extensions;
using RWS.WebAPI.Services.Mockups;
using RWS.WebAPI.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RWS.WebAPI.Extensions;

namespace RWS.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer(Configuration);
            services.AddPersistenceInfrastructure(Configuration);
            services.AddAuthenticationService(Configuration);
            services.AddControllers();
            services.AddSwagger();
            

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .SetIsOriginAllowed((host) => true);
                });
            });

            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }

            //app.UseSwagger();
            //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RWS.WebAPI v1"));

            //app.UseHttpsRedirection();


            app.UseRouting();
            app.UseSwaggerRedefined();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseErrorHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RWS.Application.Behaviours;
using System.Reflection;
using FirebaseAdmin;
using RWS.Application.Settings;
using Newtonsoft.Json;
using Google.Apis.Auth.OAuth2;

namespace RWS.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddFirebase(configuration);
        }

        private static void AddFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            var firebaseConfig = new Firebase();
            configuration.Bind("Firebase", firebaseConfig);
            var json = JsonConvert.SerializeObject(firebaseConfig);

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(json)
            });
        }
    }
}

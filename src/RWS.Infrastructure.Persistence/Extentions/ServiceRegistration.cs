using RWS.Application.Interfaces.Repositories.Common;
using RWS.Application.Interfaces.Repositories;
using RWS.Infrastructure.Persistence.Contexts;
using RWS.Infrastructure.Persistence.Repositories;
using RWS.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace RWS.Infrastructure.Persistence.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("RWSConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }


            services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddTransient<ITrainStationRepository, TrainStationRepository>();
            services.AddTransient<IIncidentRepository, IncidentRepository>();
            services.AddTransient<IIncidentTypeRepository, IncidentTypeRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IPushTokenRepository, PushTokenRepository>();
        }
    }
}

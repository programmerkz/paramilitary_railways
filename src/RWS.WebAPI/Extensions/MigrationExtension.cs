using RWS.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RWS.WebAPI.Extensions
{
    public static class MigrationExtension
    {
        public static async Task MigrateAllDbContexts(this IHost host)
        {
            //using (var scope = host.Services.CreateScope())
            //{
            //    using (var crmContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            //    {
            //        await crmContext.Database.MigrateAsync();
            //    }
            //}
        }
    }
}

using RWS.Application.Helpers.Contracts;
using RWS.Application.Interfaces;
using RWS.Infrastructure.Authentification.Models;
using RWS.Infrastructure.Authentification.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RWS.WebAPI.Extensions
{
    public static class SeedDefaultUsersExtension
    {
        public static async Task SeedDefaultUsers(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await RoleSeeder.SeedAsync(roleManager);
                await UserSeeder.SeedAsync(userManager);
            }

        }

    }
}

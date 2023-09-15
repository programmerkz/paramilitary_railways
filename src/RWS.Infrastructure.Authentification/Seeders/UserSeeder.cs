using RWS.Application.Helpers.Contracts;
using RWS.Infrastructure.Authentification.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace RWS.Infrastructure.Authentification.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByNameAsync(DefaultUsers.ADMIN_USER_NAME) == null)
            {
                var rwsAdmin = new ApplicationUser { UserName = DefaultUsers.ADMIN_USER_NAME, EmployeeId = Guid.NewGuid() };
                await userManager.CreateAsync(rwsAdmin, DefaultUsers.ADMIN_PASSWORD);
                await userManager.AddToRoleAsync(rwsAdmin, RwsRoles.ADMIN_ROLE_NAME);
            }
        }

    }
}

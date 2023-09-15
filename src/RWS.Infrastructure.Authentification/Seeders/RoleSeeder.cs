using RWS.Application.Helpers.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace RWS.Infrastructure.Authentification.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new string[] { RwsRoles.ADMIN_ROLE_NAME, RwsRoles.USER_ROLE_NAME };

            foreach (var roleName in roleNames)
                if (await roleManager.FindByNameAsync(roleName) == null)
                    await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

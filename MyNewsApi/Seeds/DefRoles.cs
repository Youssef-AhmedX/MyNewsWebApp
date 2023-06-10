using Microsoft.AspNetCore.Identity;

namespace Bookify.Web.Seeds
{
    public static class DefRoles
    {

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if(!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
        }

    }
}

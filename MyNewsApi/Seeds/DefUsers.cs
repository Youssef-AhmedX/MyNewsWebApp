using Microsoft.AspNetCore.Identity;
using MyNewsApi.Models;

namespace Bookify.Web.Seeds
{
    public class DefUsers
    {

        public static async Task SeedAdminAsync (UserManager<AppUser> userManager)
        {
            var Admin = new AppUser
            {
                Email = "admin@MyNews.com",
                FullName= "MyNews Admin",
                UserName= "admin@MyNews.com",
                EmailConfirmed = true,
                 
            };

            var user = await userManager.FindByEmailAsync(Admin.Email);

            if (user is null)
            {
                await userManager.CreateAsync (Admin, "MyNews@Admin2023");
                await userManager.AddToRoleAsync(Admin, "Admin");

            }
        }
    }
}

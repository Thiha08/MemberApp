using MemberApp.Model.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MemberApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                SeedRoles(roleManager);
                SeedUsers(userManager, roleManager);

                // Seed the database.
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // create roles
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                var user = new ApplicationUser();
                user.UserName = "Admin";
                IdentityResult userResult = userManager.CreateAsync(user, "welCome123").Result;

                var adminUser = userManager.FindByNameAsync(user.UserName).Result;
                var role = roleManager.FindByNameAsync("Admin").Result;
                if (role != null)
                {
                    IdentityResult adminRoleResult = userManager.AddToRoleAsync(adminUser, role.Name).Result;
                }
            }
        }
    }
}

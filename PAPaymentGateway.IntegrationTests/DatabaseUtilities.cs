using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAPaymentGateway.IntegrationTests
{
    public class DatabaseUtilities
    {
        /// <summary>
        /// This will seed the Identity Users/Roles and other required pre set entities.
        /// </summary>
        /// <param name="dbContext">The Database context</param>
        /// <param name="userManager">User Manager to manage users.</param>
        /// <param name="roleManager">Role Manager to manage roles.</param>
        public static void Seed(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            // Seeding Roles
            SeedRole(roleManager, "Admin").Wait();


            List<string> roles = roleManager.Roles.Select(x => x.Name).ToList();
            var adminUser = new IdentityUser()
            {
                UserName = "PAPaymentGatewayAdmin",
                NormalizedUserName = string.Empty,
                Email = string.Empty,
                NormalizedEmail = string.Empty,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (!dbContext.Users.Any(u => u.UserName == adminUser.UserName))
            {
                var password = new PasswordHasher<IdentityUser>();
                var hashed = password.HashPassword(adminUser, "PAPaymentGateway1@");
                adminUser.PasswordHash = hashed;

                var userStore = new UserStore(dbContext);
                userStore.CreateAsync(adminUser).Wait();
            }

            SeedRolesToUser(userManager, adminUser.Email, roles).Wait();
            dbContext.SaveChangesAsync().Wait();
        }

        public static async Task<IdentityResult> SeedRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            IdentityResult res = null;
            if (!roleManager.RoleExistsAsync(roleName).Result)
            {
                IdentityRole newRole = new IdentityRole(roleName);
                res = await roleManager.CreateAsync(newRole);
            }
            return res;
        }

        public static async Task<IdentityResult> SeedRolesToUser(UserManager<IdentityUser> userManager, string email, List<string> roles)
        {
            IdentityUser user = await userManager.FindByEmailAsync(email);
            var result = await userManager.AddToRolesAsync(user, roles);

            return result;
        }
    }
}

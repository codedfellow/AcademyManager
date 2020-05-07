using AcademyManager.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager
{
    public static class DefaultUserssAndRoles
    {
        public static void CreateDefaultUsersAndRoles(UserManager<AMUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            DefaultRoles(roleManager);
            DefaultUsers(userManager);
        }

        private static void DefaultUsers(UserManager<AMUser> userManager)
        {
            if (userManager.FindByNameAsync("admin@localhost.com").Result == null)
            {
                var user = new AMUser
                {
                    UserName = "admin@localhost.com",
                    Email = "admin@localhost.com"
                };
                var result = userManager.CreateAsync(user, "Elvis1$").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator");
                }
            }
        }

        private static void DefaultRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Facilitator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Facilitator"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Trainee").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Trainee"
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}

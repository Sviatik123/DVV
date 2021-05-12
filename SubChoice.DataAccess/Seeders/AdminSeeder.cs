using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using SubChoice.Core.Configuration;
using SubChoice.Core.Data.Entities;

namespace SubChoice.DataAccess.Seeders
{
    public static class AdminSeeder
    {
        public static void SeedAdmin(UserManager<User> userManager)
        {
            if (userManager.FindByEmailAsync("admin@test.com").Result == null)
            {
                var user = new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@test.com",
                    UserName = "admin@test.com",
                    IsSystemAdmin = true,
                };
                var result = userManager.CreateAsync(user, "Admin@123").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, Roles.Administrator).Wait();
                }
            }
        }
    }
}

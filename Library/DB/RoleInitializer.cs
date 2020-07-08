using Library.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.DB
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            
            if (await roleManager.FindByNameAsync("сlub member") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("сlub member"));
            }
        }
    }
}

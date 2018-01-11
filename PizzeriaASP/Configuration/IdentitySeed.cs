using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace PizzeriaASP.Configuration
{
    public class IdentitySeed
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";
        //private readonly UserManager<ApplicationUser> _userManager;


        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices
                .GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(adminUser);

            if (user == null)
            {
                user = new ApplicationUser() {UserName = adminUser};

                await userManager.CreateAsync(user, adminPassword);

                var role = "Admin";
                await userManager.AddToRoleAsync(user, role);


            }
        }
    }
}

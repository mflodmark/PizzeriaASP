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
        private const string adminPassword = "Secre123$";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices
                .GetRequiredService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(adminUser);

            if (user == null)
            {
                user = new IdentityUser("Admin");

                await userManager.CreateAsync(user, adminPassword);
            }
        }
    }
}

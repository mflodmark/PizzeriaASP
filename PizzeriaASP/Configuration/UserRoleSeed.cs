using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PizzeriaASP
{
    public class UserRoleSeed
    {
        private RoleManager<IdentityRole> _roleManager;

        public UserRoleSeed(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public void Seed()
        {
            CreateRole("Admin");
            CreateRole("PremiumUser");
            CreateRole("RegularUser");

        }

        private async void CreateRole(string name)
        {
            if (await _roleManager.FindByNameAsync(name) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = name });
            }

        }
    }
}
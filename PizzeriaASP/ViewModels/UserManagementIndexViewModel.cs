using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PizzeriaASP.ViewModels
{
    public class UserManagementIndexViewModel
    {
        public List<ApplicationUser> Users { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }
}

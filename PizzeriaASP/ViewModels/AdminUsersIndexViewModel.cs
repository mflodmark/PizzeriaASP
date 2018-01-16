using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PizzeriaASP.ViewModels
{
    public class AdminUsersIndexViewModel
    {
        public ApplicationUser User { get; set; }

        public string Role { get; set; }

    }
}

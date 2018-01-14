using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PizzeriaASP.ViewModels
{
    public class AdminUsersEditRoleViewModel
    {
        [Required]
        public ApplicationUser User { get; set; }

        [DisplayName("Current Role")]
        public string Role { get; set; }

        public List<SelectListItem> Roles { get; set; }

        [Required]
        [DisplayName("New Role")]
        public string SelectedRole { get; set; }
    }
}

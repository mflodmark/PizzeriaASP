using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    public class UserManagementController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public UserManagementController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
           var vm = new UserManagementIndexViewModel()
            {
                Users = _context.Users.OrderBy(u => u.UserName).ToList(),
                Roles = _context.Roles.Distinct().OrderBy(x => x.Name).ToList()
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRole(UserManagementIndexViewModel vm)
        {
            return View();
        }
    }
}
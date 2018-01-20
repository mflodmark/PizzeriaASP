using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Models;

namespace PizzeriaASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly TomasosContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        //Dependency Injection via konstruktorn
        public HomeController(TomasosContext context,
            UserManager<ApplicationUser> userManager
            
        )
        {
            _context = context;
            _userManager = userManager;
           
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult LoggedInIndex()
        {

            var model = _context.Kund.SingleOrDefault(x => 
                x.AnvandarNamn == _userManager.GetUserName(User));


            return View("Index", model);
        }
    }
}
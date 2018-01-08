using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private TomasosContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        //Dependency Injection via konstruktorn
        public AccountController(TomasosContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
       )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //En inloggningsmetod måste alltid tillåta anonym access
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Kund user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.AnvandarNamn, user.Losenord, true, false);

            if (result.Succeeded)
            {
                //Om inloggningen gick bra visas startsidan
                return RedirectToAction("Index", "Home", user);
            }

            return View();

        }

        //Logga av och visa startsidan
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");

        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            var unique = _context.Kund.SingleOrDefault(x => x.AnvandarNamn == register.Customer.AnvandarNamn);

            if (unique != null)
            {
                register.UniqueUsername = false;
                return View(register);
            }

            register.UniqueUsername = true;

            if (ModelState.IsValid)
            {
                //Lägger över värdena från sidan i en ApplicationUser klass
                var userIdentity = new ApplicationUser
                {
                    UserName = register.Customer.AnvandarNamn,
                    //Customer = user,
                    //CustomerId = user.KundId
                };

                //Skapar användaren i databasen
                var result = await _userManager.CreateAsync(userIdentity, register.Customer.Losenord);

                //await _userManager.AddToRoleAsync(userIdentity, "RegularUser");

                //Om det går bra loggas användaren in
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(userIdentity, isPersistent: false);

                    _context.Kund.Add(register.Customer);

                    _context.SaveChanges();

                    _context.Dispose();

                    return RedirectToAction("LoggedInIndex", "Home", register.Customer.KundId);
                }

                return View(register);
            }

            return View(register);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly TomasosContext _context;

        private readonly ApplicationDbContext _appDbContext;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        //Dependency Injection via konstruktorn
        public AccountController(TomasosContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ApplicationDbContext appDbContext
       )
        {
            _context = context;
            _appDbContext = appDbContext;
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
        public async Task<IActionResult> Login(LogInViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //true on remember me = persistense to allow cookie login
                var result = await _signInManager
                    .PasswordSignInAsync(vm.UserId, vm.Password, vm.RememberMe, false);

                if (result.Succeeded)
                {
                    //Om inloggningen gick bra visas startsidan
                    return RedirectToAction("LoggedInIndex", "Home");
                }
            }

            // Errormessage that can will be shown in the view
            ModelState.AddModelError("","Invalid Login Attempt");
            return View();

        }

        //Logga av och visa startsidan
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Session.Remove("Varukorg");

            return RedirectToAction("Index", "Home");

        }

        public IActionResult AccessDenied()
        {
            return View();
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
            var unique = _context.Kund.SingleOrDefault(x => 
                x.AnvandarNamn == register.Customer.AnvandarNamn);


            if (unique != null)
            {
                register.UniqueUsername = false;
                _context.Dispose();

                return View(register);
            }

            register.UniqueUsername = true;

            if (ModelState.IsValid)
            {
                //Lägger över värdena från sidan i en ApplicationUser klass
                var userIdentity = new ApplicationUser
                {
                    UserName = register.Customer.AnvandarNamn
                };

                //Skapar användaren i databasen
                var result = await _userManager.CreateAsync(userIdentity, register.Customer.Losenord);

                // Add default role 
                var role = "RegularUser";
                await _userManager.AddToRoleAsync(userIdentity, role);

                //Om det går bra loggas användaren in
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(userIdentity, isPersistent: false);

                    _context.Kund.Add(register.Customer);

                    _context.SaveChanges();
                    _context.Dispose();

                    return RedirectToAction("LoggedInIndex", "Home", register.Customer.KundId);
                }

                _context.Dispose();
                return View(register);
            }

            _context.Dispose();
            return View(register);
        }

        public IActionResult EditUser(string username)
        {
            var customer = _context.Kund.Single(x => x.AnvandarNamn == username);

            var model = new RegisterViewModel()
            {
                Customer = customer,
                UniqueUsername = true,
                KeepCurrentPassword = GetPw()
            };

            //Set(true);

           

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(RegisterViewModel vm)
        {
            var unique = _context.Kund.SingleOrDefault(x =>
                x.AnvandarNamn == vm.Customer.AnvandarNamn);

            // Must check current username as well
            if (unique != null && unique.AnvandarNamn != _userManager.GetUserName(User))
            {
                vm.UniqueUsername = false;
                _context.Dispose();

                return View(vm);
            }

            if (ModelState.IsValid)
            {
                var p = _context.Kund.Find(vm.Customer.KundId);

                _context.Entry(p).CurrentValues.SetValues(vm.Customer);

                // Save new username
                var user = await _userManager.GetUserAsync(User);

                await _userManager.SetUserNameAsync(user, vm.Customer.AnvandarNamn);

                // Save new password
                if (vm.KeepCurrentPassword == false)
                {
                    //var store = new UserStore<ApplicationUser>(_appDbContext);
                    //var manager = new UserManager<ApplicationUser>(store);

                    //var newPasswordHash = manager.PasswordHasher.HashPassword(user, vm.Customer.Losenord);
                    //await store.SetPasswordHashAsync(user, newPasswordHash);
                    //await manager.UpdateAsync(user);

                    //var result = await _userManager.CreateAsync(user, vm.Customer.Losenord);

                    //if (result.Succeeded)
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);

                    //    return RedirectToAction("LoggedInIndex", "Home");
                    //}
                }
                
                _context.SaveChanges();
                _context.Dispose();

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult KeepPassword(RegisterViewModel vm)
        {
            vm.KeepCurrentPassword = false;

            return PartialView("_KeepPassword", vm);
        }

        private void Set(bool newPasswordOrNot)
        {
            var temp = JsonConvert.SerializeObject(newPasswordOrNot);
            HttpContext.Session.SetString("pw", temp);
        }

        private bool GetPw()
        {
            bool pw;
            if (HttpContext.Session.GetString("pw") == null)
            {
                pw = true;
            }
            else
            {
                var serializedValue = HttpContext.Session.GetString("pw");
                pw = JsonConvert.DeserializeObject<bool>(serializedValue);
            }

            return pw;
        }
    }
}
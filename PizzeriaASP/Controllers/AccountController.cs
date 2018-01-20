using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;

        public AccountController(ICustomerRepository context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IPasswordHasher<ApplicationUser> passwordHash)
        {
            passwordHasher = passwordHash;
            _customerRepository = context;
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

        //Logga ut och visa startsidan
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
            //var unique = _context.Kund.SingleOrDefault(x => 
            //    x.AnvandarNamn.ToLower() == register.Customer.AnvandarNamn.ToLower());

            var unique = _customerRepository.GetSingleCustomer(register.Customer.AnvandarNamn);

            // Check unique username
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

                    _customerRepository.SaveCustomer(register.Customer);

                    return RedirectToAction("LoggedInIndex", "Home", register.Customer.KundId);
                }
            }

            return View(register);
        }

        public IActionResult EditUser(string username)
        {
            var customer = _customerRepository.GetSingleCustomer(username);

            var model = new RegisterViewModel()
            {
                Customer = customer,
                UniqueUsername = true,
                KeepCurrentPassword = GetPw()
            };
          
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(RegisterViewModel vm)
        {
            var unique = _customerRepository.GetSingleCustomer(vm.Customer.AnvandarNamn);

            // Must check current username as well
            if (unique != null && unique.AnvandarNamn != _userManager.GetUserName(User))
            {
                vm.UniqueUsername = false;

                return View(vm);
            }

            if (ModelState.IsValid)
            {
                _customerRepository.SaveCustomer(vm.Customer);

                // Save new username
                var user = await _userManager.GetUserAsync(User);

                await _userManager.SetUserNameAsync(user, vm.Customer.AnvandarNamn);

                // Save new password
                if (vm.KeepCurrentPassword == false)
                {
                    // Get password
                    user.PasswordHash = passwordHasher.HashPassword(user, vm.Customer.Losenord);

                    // Update password
                    await _userManager.UpdateAsync(user);
                }
                
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //public ActionResult KeepPasswordTrue(RegisterViewModel vm)
        //{
        //    vm.KeepCurrentPassword = true;

        //    return PartialView("_KeepPassword", vm);
        //}
         
        public ActionResult KeepPasswordFalse(RegisterViewModel vm)
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

        [AllowAnonymous]
        public IActionResult GoogleLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account", new {ReturnUrl = returnUrl});

            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                var user = new ApplicationUser()
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                var identResult = await _userManager.CreateAsync(user);

                if (identResult.Succeeded)
                {
                    identResult = await _userManager.AddLoginAsync(user, info);

                    // Add user to customer table as well
                    var customer = new Kund()
                    {
                        Email = user.Email,
                        AnvandarNamn = user.Email,
                        Gatuadress = "N/A",
                        Postnr = "N/A",
                        Postort = "N/A",
                        Telefon = "N/A",
                        Losenord = "Google",
                        Namn = user.Email
                    };

                    _customerRepository.SaveCustomer(customer);

                    // Add default role 
                    var role = "RegularUser";
                    await _userManager.AddToRoleAsync(user, role);

                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);

                        return Redirect(returnUrl);
                    }
                }

                return AccessDenied();
            }
        }
    }
}
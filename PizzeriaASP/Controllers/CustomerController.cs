using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    public class CustomerController : Controller
    {
        private TomasosContext _context;

        public CustomerController(TomasosContext context)
        {
            _context = context;
        }


        public IActionResult New()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Register(RegisterModel registerModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        registerModel.Post.Date = DateTime.Now;

        //        _context.Kund.Add(registerModel.Post);

        //        _context.SaveChanges();

        //        _context.Dispose();

        //        return RedirectToAction("New");
        //    }

        //    return View("Add", registerModel);
        //}

        public IActionResult LogIn()
        {
            var model = new LogInViewModel() { LogInFailed = false };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TryingToLogIn(LogInViewModel logIn)
        {
            var customer = _context.Kund.SingleOrDefault(x =>
                x.AnvandarNamn == logIn.UserId && x.Losenord == logIn.Password);

            _context.Dispose();

            if (customer != null)
            {
                return View("LoggedIn", customer);
            }

            logIn.LogInFailed = true;

            return View("LogIn", logIn);
        }

        public IActionResult LoggedIn()
        {
            return View();
        }
    }
}
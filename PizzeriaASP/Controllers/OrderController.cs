using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaASP.Models;

namespace PizzeriaASP.Controllers
{
    public class OrderController : Controller
    {

        //private IOrderRepository repository;
        //private Bestallning cart;

        //public OrderController(IOrderRepository repo, Bestallning cartService)
        //{
        //    repository = repo;
        //    cart = cartService;
        //}

        private readonly TomasosContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        //Dependency Injection via konstruktorn
        public OrderController(TomasosContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public ViewResult CheckOut() => View(new Bestallning());

        //[HttpPost]
        public IActionResult CheckOut(Bestallning order)
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                //order.Lines = cart.Lines.ToArray();
                //repository.SaveOrder(order);

                order.BestallningDatum = DateTime.Now;
                order.Kund = _context.Kund.Single(x => x.AnvandarNamn == _userManager.GetUserName(User));
                order.Totalbelopp = order.ComputeTotalValue();

                _context.Bestallning.Add(order);

                _context.SaveChanges();

                _context.Dispose();

                return RedirectToAction("Completed");
            }
            else
            {
                return RedirectToAction("Index","Cart");
            }
        }

        public ViewResult Completed()
        {
            //cart.Clear();
            HttpContext.Session.Clear();
            return View();
        }

        private List<BestallningMatratt> GetCart()
        {
            List<BestallningMatratt> prodList;
            if (HttpContext.Session.GetString("Varukorg") == null)
            {
                prodList = new List<BestallningMatratt>();
            }
            else
            {
                var serializedValue = HttpContext.Session.GetString("Varukorg");
                prodList = JsonConvert.DeserializeObject<List<BestallningMatratt>>(serializedValue);
            }

            return prodList;
        }

    }

}
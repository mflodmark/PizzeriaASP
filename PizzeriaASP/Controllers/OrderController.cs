using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

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
        private readonly ApplicationDbContext _contextUserMgm;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //Dependency Injection via konstruktorn
        public OrderController(TomasosContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext contextUserMgm
        )
        {
            _contextUserMgm = contextUserMgm;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut()
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            // Behövs denna?
            if (ModelState.IsValid)
            {
                var customer = _context.Kund.Single(x => x.AnvandarNamn == _userManager.GetUserName(User));

                var order = new Bestallning
                {
                    BestallningDatum = DateTime.Now,
                    KundId = customer.KundId,
                    Levererad = false,
                    Totalbelopp = cart.Sum(e => e.Matratt.Pris * e.Antal)
                };

                _context.Bestallning.Add(order);

                // Save order
                _context.SaveChanges();

                foreach (var c in cart)
                {
                    _context.BestallningMatratt.Add(new BestallningMatratt()
                    {
                        BestallningId = order.BestallningId,
                        MatrattId = c.MatrattId,
                        Antal = c.Antal
                    });
                }

                // Add points to Premium users
                var userMgm = await _userManager.GetUserAsync(User);

                if (_signInManager.UserManager.IsInRoleAsync(userMgm, "PremiumUser").Result)
                {
                    var user = _context.Kund.Single(x => x.AnvandarNamn == _userManager.GetUserName(User));
                    user.Poang += cart.Sum(x => x.Antal) * 10;
                }
                
                // Save orderlist
                _context.SaveChanges();

                _context.Dispose();

                // Add values to pass to order confirmation
                order.Kund = customer;
                order.BestallningMatratt = cart;

                HttpContext.Session.Clear();

                return View("Completed", order);
                //return RedirectToAction("Completed", order);
            }
            else
            {
                return RedirectToAction("Index","Cart");
            }
        }

        public ViewResult Completed(Bestallning order)
        {
            return View(order);
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
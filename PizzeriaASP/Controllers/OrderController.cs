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
using StackExchange.Redis;

namespace PizzeriaASP.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //Dependency Injection via konstruktorn
        public OrderController(ICustomerRepository context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = context;
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
                var customer = _customerRepository.GetSingleCustomer(_userManager.GetUserName(User));

                var newOrder = new Bestallning
                {
                    BestallningDatum = DateTime.Now,
                    KundId = customer.KundId,
                    Levererad = false,
                    Totalbelopp = cart.Sum(e => e.Matratt.Pris * e.Antal),
                };

                _orderRepository.SaveOrder(newOrder, cart);

                // Add points to Premium users
                var userMgm = await _userManager.GetUserAsync(User);

                if (_signInManager.UserManager.IsInRoleAsync(userMgm, "PremiumUser").Result)
                {
                    var user = _customerRepository.GetSingleCustomer(_userManager.GetUserName(User));

                    user.Poang += cart.Sum(x => x.Antal) * 10;

                    _customerRepository.SaveCustomer(user);

                }
                

                // Add values to pass to order confirmation
                newOrder.Kund = customer;
                newOrder.BestallningMatratt = cart;

                HttpContext.Session.Clear();

                return View("Completed", newOrder);               
            }

            return RedirectToAction("Index","Cart");
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
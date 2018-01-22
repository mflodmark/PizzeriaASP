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

            if (!cart.BestallningMatratt.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            // Behövs denna?
            if (ModelState.IsValid)
            {
                var customer = _customerRepository.GetSingleCustomer(_userManager.GetUserName(User));

                cart.BestallningDatum = DateTime.Now;
                cart.KundId = customer.KundId;
                cart.Levererad = false;

                var orderList = cart.BestallningMatratt.ToList();
                
                _orderRepository.SaveOrder(cart, orderList);

                // Add points to Premium users
                var userMgm = await _userManager.GetUserAsync(User);

                if (_signInManager.UserManager.IsInRoleAsync(userMgm, "PremiumUser").Result)
                {
                    var user = _customerRepository.GetSingleCustomer(_userManager.GetUserName(User));

                    // Add points for each product bought
                    user.Poang += cart.BestallningMatratt.Sum(x => x.Antal) * 10;

                    // Clear points that have been used
                    if (user.Poang >= 100)
                    {
                        user.Poang -= 100;
                    }

                    _customerRepository.SaveCustomer(user);

                }
                

                // Add values to pass to order confirmation
                cart.Kund = customer;
                cart.BestallningMatratt = orderList;
                

                HttpContext.Session.Clear();

                return View("Completed", cart);               
            }

            return RedirectToAction("Index","Cart");
        }

        public ViewResult Completed(Bestallning order)
        {
            return View(order);
        }

        private Bestallning GetCart()
        {
            Bestallning order;
            if (HttpContext.Session.GetString("Varukorg") == null)
            {
                order = new Bestallning();
            }
            else
            {
                var serializedValue = HttpContext.Session.GetString("Varukorg");
                order = JsonConvert.DeserializeObject<Bestallning>(serializedValue);
            }

            return order;
        }

    }

}
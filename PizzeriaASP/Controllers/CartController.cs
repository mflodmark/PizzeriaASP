using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using PizzeriaASP.Infrastructure;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IIdentityRepository _identityRepo;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public CartController(IProductRepository context, UserManager<ApplicationUser> userManager,
            IIdentityRepository identityRepo, ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _identityRepo = identityRepo;
            _productRepository = context;
            _userManager = userManager;
        }

        public ViewResult Index(string returnUrl)
        {
            // Get session value
            var order = GetCart();

            var cart = order;

            var points = _customerRepository.GetSingleCustomer(_userManager.GetUserName(User)).Poang;

            if (cart.BestallningMatratt.Any())
            {
                return View(new CartIndexViewModel()
                {
                    Cart = cart,
                    ReturnUrl = returnUrl,
                    CartTotalValue = cart.ComputeTotalValue(GetUserRole(), points, cart.BestallningMatratt.Sum(p=>p.Antal)),
                    CartRebate = cart.GetRebate(GetUserRole()),
                    Points = points
                });
            }

            return View(new CartIndexViewModel()
            {
                Cart = cart,
                ReturnUrl = returnUrl,
                CartTotalValue = 0,
                CartRebate = 0,
                Points = points
            });

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public PartialViewResult AddToCart(int productId, string returnUrl)
        {
            var product = _productRepository.GetSingleProduct(productId);

            var order = GetCart();

            // Check if product exist in cart => add qty 1
            if (order.BestallningMatratt.Any(x => x.MatrattId == productId))
            {
                order.BestallningMatratt.Single(x => x.MatrattId == productId).Antal += 1;
            }
            else
            {
                var newProd = new BestallningMatratt()
                {
                    Antal = 1,
                    Matratt = new Matratt()
                    {
                        MatrattNamn = product.MatrattNamn,
                        Pris = product.Pris
                    }, // Selfreference without this line
                    MatrattId = productId
                };
                order.BestallningMatratt.Add(newProd);
            }

            var points = _customerRepository.GetSingleCustomer(_userManager.GetUserName(User)).Poang;

            order.Totalbelopp = order.ComputeTotalValue(GetUserRole(), points, order.BestallningMatratt.Sum(p => p.Antal));

            SetCart(order);
            
            var model = new CartIndexViewModel()
            {
                Cart = order,
                CartTotalValue = order.Totalbelopp
            };

            return PartialView("_CartSumPartial", model);
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

        private void SetCart(Bestallning newOrder)
        {
            var temp = JsonConvert.SerializeObject(newOrder);
            HttpContext.Session.SetString("Varukorg", temp);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = _productRepository.GetSingleProduct(productId);

            if (product != null)
            {
                var cart = GetCart();

                // Check for points and compute total value
                var points = _customerRepository.GetSingleCustomer(_userManager.GetUserName(User)).Poang;

                cart.Totalbelopp = cart.ComputeTotalValue(GetUserRole(), points, cart.BestallningMatratt.Sum(p => p.Antal));
                
                // Get orderList
                var list = cart.BestallningMatratt.ToList();

                list.RemoveAll(p => p.MatrattId == product.MatrattId);

                cart.BestallningMatratt = list;

                SetCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        private string GetUserRole()
        {
            // Get user & role
            var getUser = _identityRepo.GetSingleUser(_userManager.GetUserName(User));

            var userRoles = _userManager.GetRolesAsync(getUser).Result;

            return userRoles[0];
        }

    }
}
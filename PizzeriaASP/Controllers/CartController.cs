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
        //private readonly IProductRepository _repository;
        //private readonly Bestallning _cart;
        private readonly TomasosContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //public CartController(IProductRepository repo, Bestallning cartService)
        //{
        //    _repository = repo;
        //    _cart = cartService;
        //}

        public CartController(TomasosContext context, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public ViewResult Index(string returnUrl)
        {
            // Get session value
            var prodList = GetCart();

            var cart = new Bestallning() {BestallningMatratt = prodList};

            return View(new CartIndexViewModel()
            {
                Cart = cart,
                ReturnUrl = returnUrl,
                CartTotalValue = prodList.Sum(e => e.Matratt.Pris * e.Antal)
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            var product = _context.Matratt
                .FirstOrDefault(p => p.MatrattId == productId);

            var prodList = GetCart();

            // Check if product exist in cart => add qty 1
            if (prodList.Any(x => x.MatrattId == productId))
            {
                prodList.Single(x => x.MatrattId == productId).Antal += 1;
            }
            else
            {
                var newProd = new BestallningMatratt()
                {
                    Antal = 1,
                    Matratt = product,
                    MatrattId = productId
                };
                prodList.Add(newProd);
            }

            SetCart(prodList);

            return RedirectToAction("List","Product");
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

        private void SetCart(List<BestallningMatratt> newList)
        {
            var temp = JsonConvert.SerializeObject(newList);
            HttpContext.Session.SetString("Varukorg", temp);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = _context.Matratt.FirstOrDefault(p => p.MatrattId == productId);

            if (product != null)
            {
                var cart = GetCart();

                cart.RemoveAll(p => p.Matratt.MatrattId == product.MatrattId);

                SetCart(cart);
                //_cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

    }
}
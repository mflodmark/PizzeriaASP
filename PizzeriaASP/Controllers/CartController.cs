using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Infrastructure;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private Bestallning cart;

        public CartController(IProductRepository repo, Bestallning cartService)
        {
            repository = repo;
            cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel()
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public RedirectToActionResult AddToCart(int matrattId, string returnUrl)
        {
            var product = repository.Products
                .FirstOrDefault( p => p.MatrattId == matrattId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = repository.Products.FirstOrDefault(p => p.MatrattId == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

    }
}
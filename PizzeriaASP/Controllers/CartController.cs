using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Infrastructure;
using PizzeriaASP.Models;

namespace PizzeriaASP.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;

        public CartController(IProductRepository repo)
        {
            repository = repo;
        }

        public IActionResult AddToCart(int productId, string returnUrl)
        {
            var product = repository.Products
                .FirstOrDefault( p => p.MatrattId == productId);

            if (product != null)
            {
                var cart = GetCart();

                
                
            }
                
            return View();
        }

        private BestallningMatratt GetCart()
        {
            var cart = HttpContext.Session.GetJson<BestallningMatratt>("Cart") ?? 
                new BestallningMatratt();

            return cart;
        }

        private void SaveCart(BestallningMatratt cart)
        {
            HttpContext.Session.SetJson("Cart", cart);           
        }
    }
}
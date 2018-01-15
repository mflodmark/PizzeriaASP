using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Components
{
    public class CartSummaryViewComponent: ViewComponent
    {
     

        private Bestallning GetCart()
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

            var order = new Bestallning() {BestallningMatratt = prodList};

            return order;
        }

        public IViewComponentResult Invoke()
        {
            var cart = GetCart();

            var model = new CartIndexViewModel()
            {
                Cart = cart,
                CartTotalValue = cart.BestallningMatratt.Sum(e => e.Matratt.Pris * e.Antal)
            };

            return View(model);
        }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaASP.Models;

namespace PizzeriaASP.Components
{
    public class CartSummaryViewComponent: ViewComponent
    {
        //private readonly Bestallning _cart;

        //public CartSummaryViewComponent(Bestallning cartService)
        //{
        //    _cart = cartService;
        //}

        //public IViewComponentResult Invoke()
        //{
        //    return View(_cart);
        //}

        

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

            return View(GetCart());
        }
    }
}

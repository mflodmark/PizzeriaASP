using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Models;

namespace PizzeriaASP.Components
{
    public class CartSummaryViewComponent:ViewComponent
    {
        private Bestallning cart;

        public CartSummaryViewComponent(Bestallning cartService)
        {
            cart = cartService;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}

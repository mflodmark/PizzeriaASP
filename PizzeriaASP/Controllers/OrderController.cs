using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Models;

namespace PizzeriaASP.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Bestallning cart;

        public OrderController(IOrderRepository repo, Bestallning cartService)
        {
            repository = repo;
            cart = cartService;
        }

        public ViewResult CheckOut() => View(new Bestallning());

        [HttpPost]
        public IActionResult CheckOut(Bestallning order)
        {
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                //order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);

                return RedirectToAction("Completed");
            }
            else
            {
                return View();
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }

    }

}
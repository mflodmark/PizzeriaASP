using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;
using SQLitePCL;

namespace PizzeriaASP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCartsController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public AdminCartsController(IOrderRepository context)
        {
            _orderRepository = context;
        }

        public IActionResult Index()
        {
            var model = new AdminCartViewModel()
            {
                Orders = _orderRepository.Orders.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCart(int id)
        {
            _orderRepository.DeleteOrder(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(int id)
        {
            _orderRepository.UpdateDeliveryStatus(id, true);

            return RedirectToAction("Index");
        }
    }
}
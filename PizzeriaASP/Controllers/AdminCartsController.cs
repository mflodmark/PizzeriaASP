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
        private readonly TomasosContext _context;

        public AdminCartsController(TomasosContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new AdminCartViewModel()
            {
                Orders = _context.Bestallning.Include(x => x.Kund).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCart(int id)
        {
            var order = _context.Bestallning.Single(x => x.BestallningId == id);

            foreach (var item in order.BestallningMatratt)
            {
                _context.BestallningMatratt.Remove(item);
            }

            _context.SaveChanges();

            _context.Remove(order);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCart(int id)
        {
            var order = _context.Bestallning.Single(x => x.BestallningId == id);

            order.Levererad = true;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
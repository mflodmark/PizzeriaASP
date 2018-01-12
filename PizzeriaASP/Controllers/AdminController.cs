using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly TomasosContext _context;

        //private readonly IProductRepository repository;

        //public AdminController(IProductRepository repo)
        //{
        //    repository = repo;
        //}

        public AdminController(TomasosContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = _context.Matratt.Select(p => new AdminIndexViewModel()
            {
                Product = p,
                ProductType = p.MatrattTypNavigation.Beskrivning
            }).ToList();

            return View(model);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int productId)
        {
            if (ModelState.IsValid)
            {
                var product = _context.Matratt.Find(productId);

                _context.Remove(product);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        public IActionResult Edit(int productId)
        {
            var model = new AdminEditViewModel()
            {
                Product = _context.Matratt.FirstOrDefault(p => p.MatrattId == productId),
                ProductTypes = GetProductTypes()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Matratt product)
        {
            if (ModelState.IsValid)
            {
                var p = _context.Matratt.Find(product.MatrattId);

                if (p == null)
                {
                    // New
                    _context.Add(new Matratt()
                    {
                        Beskrivning = product.Beskrivning,
                        MatrattTyp = product.MatrattTyp,
                        Pris = product.Pris,
                        MatrattNamn = product.MatrattNamn
                    });
                }
                else
                {
                    // Old
                    p.MatrattTyp = product.MatrattTyp;
                    p.Beskrivning = product.Beskrivning;
                    p.Pris = product.Pris;
                    p.MatrattNamn = product.MatrattNamn;
                }
                
                _context.SaveChanges();

                _context.Dispose();

                return RedirectToAction("Index");
            }

            return View(new AdminEditViewModel()
            {
                Product = product,
                ProductTypes = GetProductTypes()
            });
        }

        public IActionResult Create() => View("Edit", new AdminEditViewModel()
        {
            Product = new Matratt(),
            ProductTypes = GetProductTypes()
        });

        private List<SelectListItem> GetProductTypes()
        {
            return _context.MatrattTyp.Select(p => new SelectListItem()
            {
                Value = p.MatrattTyp1.ToString(),
                Text = p.Beskrivning
            }).OrderBy(o => o.Text).ToList();
        }
    }
}
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
        public IActionResult DeleteProduct(int productId)
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

        public IActionResult EditProduct(int productId)
        {
            var model = new AdminEditViewModel()
            {
                Product = _context.Matratt.FirstOrDefault(p => p.MatrattId == productId),
                ProductTypes = GetProductTypes(),
                OptionalIngredientsList = GetIngredients(),
                IngredientList = GetIngredients(productId)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Matratt product)
        {
            if (ModelState.IsValid)
            {
                var p = _context.Matratt.Find(product.MatrattId);

                _context.Entry(p).CurrentValues.SetValues(product);
   
                _context.SaveChanges();

                _context.Dispose();

                return RedirectToAction("Index");
            }

            // Indata will resist in view if not modelstat.clear is used
            return View();
        }

        public IActionResult AddProduct(Matratt product)
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
                _context.Dispose();

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult AddProduct() => View(new AdminEditViewModel()
        {
            Product = new Matratt(),
            ProductTypes = GetProductTypes()
        });


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddIngredient(AdminEditViewModel vm)
        {
            var ingredients = vm.IngredientList;
            var ingredient = _context.Produkt.Find(vm.SelectedIngredientId);

            ingredients.Add(ingredient);

            return View("EditProduct", new AdminEditViewModel()
            {
                Product = vm.Product,
                ProductTypes = GetProductTypes(),
                OptionalIngredientsList = GetIngredients(),
                IngredientList = ingredients
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveIngredient(AdminEditViewModel vm)
        {
            var ingredients = vm.IngredientList;
            var ingredient = _context.Produkt.Find(vm.SelectedIngredientId);

            ingredients.Remove(ingredient);

            return View("Edit", new AdminEditViewModel()
            {
                Product = vm.Product,
                ProductTypes = GetProductTypes(),
                OptionalIngredientsList = GetIngredients(),
                IngredientList = ingredients
            });
        }

        private List<SelectListItem> GetProductTypes()
        {
            return _context.MatrattTyp.Select(p => new SelectListItem()
            {
                Value = p.MatrattTyp1.ToString(),
                Text = p.Beskrivning
            }).OrderBy(o => o.Text).ToList();
        }

        private List<SelectListItem> GetIngredients()
        {
            return _context.Produkt.Select(p => new SelectListItem()
            {
                Value = p.ProduktId.ToString(),
                Text = p.ProduktNamn
            }).OrderBy(o => o.Text).ToList();
        }

        private List<Produkt> GetIngredients(int id)
        {
            var i = _context.MatrattProdukt.Where(x => x.MatrattId == id).Select(y => y.Produkt).ToList();

            return i;
        }
    }
}
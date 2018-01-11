using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index() => 
            View(new ProductsListViewModel
        {
            Products = repository.Products,Categories = repository.Categories
        });

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            return View("Index");
        }

        public IActionResult Edit(int productId)
        {
            var model = repository.Products.FirstOrDefault(p => p.MatrattId == productId);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Matratt product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);

                return RedirectToAction("Index");
            }

            return View(product);
        }

        public IActionResult Create(int productId)
        {
            return View("Index");
        }
    }
}
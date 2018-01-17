using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : Controller
    {
        private readonly IProductRepository _repository;

        public AdminProductsController(IProductRepository repo)
        {
            _repository = repo;
        }

        public IActionResult Index()
        {
            var model = _repository.Products.Select(p => new AdminIndexViewModel()
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
                _repository.DeleteProduct(productId);

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        public IActionResult EditProduct(int productId)
        {
            var product = _repository.Products
                .Where(p => p.MatrattId == productId)
                .Include(p => p.MatrattProdukt)
                .ThenInclude(p => p.Produkt)
                .FirstOrDefault();

            var model = new AdminEditViewModel()
            {
                Product = product,
                ProductTypes = _repository.GetProductTypes(),
                OptionalIngredientsList = _repository.GetIngredients(),
                IngredientList = _repository.GetIngredients(productId)
            };

            //Add current ingredients to session
            //var i = new List<Produkt>();

            //foreach (var item in product.MatrattProdukt)
            //{
            //    i.Add(item.Produkt);
            //}

            //HttpContext.Session.Clear();

            //GetIngredientList();
            //SetIngredientList(i);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(Matratt product)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveProduct(product);
                
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(Matratt product)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveProduct(product);

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult AddProduct() => View(new AdminEditViewModel()
        {
            Product = new Matratt(),
            ProductTypes = _repository.GetProductTypes(),
            OptionalIngredientsList = _repository.GetIngredients(),
            IngredientList = new List<Produkt>()
        });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddIngredient(AdminEditViewModel vm)
        {
            var model = GetIngredientList();

            model.Add(_repository.GetSingleIngredient(vm.SelectedIngredientId));

            SetIngredientList(model);

            return PartialView("_EditAddIngredientPartial", model);
        }

        private List<Produkt> GetIngredientList()
        {
            List<Produkt> prodList;
            if (HttpContext.Session.GetString("IngredientList") == null)
            {
                prodList = new List<Produkt>();
            }
            else
            {
                var serializedValue = HttpContext.Session.GetString("IngredientList");
                prodList = JsonConvert.DeserializeObject<List<Produkt>>(serializedValue);
            }

            return prodList;
        }

        private void SetIngredientList(List<Produkt> newList)
        {
            var temp = JsonConvert.SerializeObject(newList);
            HttpContext.Session.SetString("IngredientList", temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveIngredient(AdminEditViewModel vm)
        {
            var ingredients = vm.IngredientList;

            ingredients.Remove(_repository.GetSingleIngredient(vm.SelectedIngredientId));

            return View("EditProduct", new AdminEditViewModel()
            {
                Product = vm.Product,
                ProductTypes = _repository.GetProductTypes(),
                OptionalIngredientsList = _repository.GetIngredients(),
                IngredientList = ingredients
            });
        }




    }
}
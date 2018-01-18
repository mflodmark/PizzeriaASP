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
            HttpContext.Session.Clear();

            var product = _repository.Products
                .Where(p => p.MatrattId == productId)
                .Include(p => p.MatrattProdukt)
                .ThenInclude(p => p.Produkt)
                .FirstOrDefault();

            var model = new AdminEditViewModel()
            {
                Product = product,
                ProductTypes = _repository.GetProductTypes(),
                OptionalIngredientsList = _repository.GetOptionalIngredients(productId),
                IngredientList = _repository.GetCurrentIngredients(productId)
            };

            return View("EditOrAddProduct", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOrAddProduct(Matratt product)
        {

            if (ModelState.IsValid)
            {
                _repository.SaveProduct(product);

                var ingredientList = GetIngredientList(product.MatrattId);
                
                _repository.SaveIngredientList(product.MatrattId, ingredientList);
                
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult AddProduct()
        {
            HttpContext.Session.Clear();

            var model = new AdminEditViewModel()
            {
                Product = new Matratt(),
                ProductTypes = _repository.GetProductTypes(),
                OptionalIngredientsList = _repository.GetAllIngredients(),
                IngredientList = new List<Produkt>()
            };

            return View("EditOrAddProduct", model);
        } 

        public IActionResult AddIngredient(AdminEditViewModel vm)
        {
            var ingredients = GetIngredientList(vm.SelectedProductId);

            ingredients.Add(_repository.GetSingleIngredient(vm.SelectedIngredientId));

            SetIngredientList(ingredients);

            var model = new AdminEditViewModel()
            {
                Product = _repository.GetSingleProduct(vm.SelectedProductId),
                ProductTypes = _repository.GetProductTypes(),
                OptionalIngredientsList = _repository.GetOptionalIngredients(vm.SelectedProductId),
                IngredientList = ingredients
            };

            return PartialView("_EditAddIngredientPartial", model);
        }
        
        public IActionResult RemoveIngredient(AdminEditViewModel vm)
        {
            var ingredients = GetIngredientList(vm.SelectedProductId);

            var i = _repository.GetSingleIngredient(vm.SelectedIngredientId);

            ingredients.Remove(i);

            SetIngredientList(ingredients);

            var model = new AdminEditViewModel()
            {
                Product = _repository.GetSingleProduct(vm.SelectedProductId),
                ProductTypes = _repository.GetProductTypes(),
                OptionalIngredientsList = _repository.GetOptionalIngredients(vm.SelectedProductId),
                IngredientList = ingredients
            };

            return PartialView("_EditAddIngredientPartial", model);
        }

        private List<Produkt> GetIngredientList(int productId)
        {
            List<Produkt> prodList;
            if (HttpContext.Session.GetString("IngredientList") == null)
            {
                prodList = _repository.GetCurrentIngredients(productId);
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


    }
}
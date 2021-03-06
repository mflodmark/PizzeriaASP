﻿using System;
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
        private readonly IProductRepository _productRepository;

        public AdminProductsController(IProductRepository repo)
        {
            _productRepository = repo;
        }

        public IActionResult Index()
        {
            var model = _productRepository.Products
                .Include(p=>p.MatrattTypNavigation)
                .Select(p => new AdminIndexViewModel()
                {
                    Product = p,
                    ProductType = p.MatrattTypNavigation.Beskrivning,
                })
                .OrderBy(p=>p.Product.MatrattNamn)
                .ToList();

            return View(model);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int productId)
        {
            if (ModelState.IsValid)
            {
                _productRepository.DeleteProduct(productId);

                return RedirectToAction("Index");
            }

            return View("Index");
        }

        public IActionResult EditProduct(int productId)
        {
            HttpContext.Session.Clear();

            var product = _productRepository.Products
                .Where(p => p.MatrattId == productId)
                .Include(p => p.MatrattProdukt)
                .ThenInclude(p => p.Produkt)
                .FirstOrDefault();

            var model = new AdminEditViewModel()
            {
                Product = product,
                ProductTypes = _productRepository.GetProductTypes(),
                OptionalIngredientsList = _productRepository.GetOptionalIngredients(productId, GetIngredientList(productId)),
                IngredientList = _productRepository.GetCurrentIngredients(productId)
            };

            return View("EditOrAddProduct", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOrAddProduct(Matratt product)
        {
            if (_productRepository.CheckUniqueValue(product.MatrattNamn, product.MatrattId) == false)
            {
                ModelState.AddModelError("MatrattNamn", "Name must be unique");
            }

            if (ModelState.IsValid)
            {
                _productRepository.SaveProduct(product);

                var ingredientList = GetIngredientList(product.MatrattId);
                
                _productRepository.SaveIngredientList(product.MatrattId, ingredientList);

                HttpContext.Session.Clear();

                return RedirectToAction("Index");
            }

            var model = new AdminEditViewModel()
            {
                Product = product,
                ProductTypes = _productRepository.GetProductTypes(),
                OptionalIngredientsList = _productRepository.GetAllIngredients(),
                IngredientList = GetIngredientList(0)
            };

            return View(model);
        }

        public IActionResult AddProduct()
        {
            HttpContext.Session.Clear();

            var model = new AdminEditViewModel()
            {
                Product = new Matratt(),
                ProductTypes = _productRepository.GetProductTypes(),
                OptionalIngredientsList = _productRepository.GetAllIngredients(),
                IngredientList = GetIngredientList(0)
            };

            return View("EditOrAddProduct", model);
        } 

        public PartialViewResult AddIngredient(AdminEditViewModel vm)
        {
            var ingredients = GetIngredientList(vm.SelectedProductId);

            ingredients.Add(_productRepository.GetSingleIngredient(vm.SelectedIngredientId));

            SetIngredientList(ingredients);

            var model = new AdminEditViewModel()
            {
                Product = _productRepository.GetSingleProduct(vm.SelectedProductId),
                ProductTypes = _productRepository.GetProductTypes(),
                OptionalIngredientsList = _productRepository.GetOptionalIngredients(vm.SelectedProductId , ingredients),
                IngredientList = ingredients
            };

            return PartialView("_EditAddIngredientPartial", model);
        }
        
        public PartialViewResult RemoveIngredient(AdminEditViewModel vm)
        {
            var ingredients = GetIngredientList(vm.SelectedProductId);

            var i = _productRepository.GetSingleIngredient(vm.SelectedIngredientId);

            // Check for value in list - Remove doesn't work..
            for (int j = 0; j < ingredients.Count; j++)
            {
                if (ingredients[j].ProduktId == i.ProduktId)
                {
                    ingredients.RemoveAt(j);
                    break;
                }
                
            }

            SetIngredientList(ingredients);

            var model = new AdminEditViewModel()
            {
                Product = _productRepository.GetSingleProduct(vm.SelectedProductId),
                ProductTypes = _productRepository.GetProductTypes(),
                OptionalIngredientsList = _productRepository.GetOptionalIngredients(vm.SelectedProductId, ingredients),
                IngredientList = ingredients
            };

            return PartialView("_EditAddIngredientPartial", model);
        }

        private List<Produkt> GetIngredientList(int productId)
        {
            List<Produkt> prodList;
            if (HttpContext.Session.GetString("IngredientList") == null)
            {
                prodList = _productRepository.GetCurrentIngredients(productId);
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
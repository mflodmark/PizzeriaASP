﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.ResultOperators.Internal;
using PizzeriaASP.Models;

namespace PizzeriaASP.Controllers
{
    public class AdminIngredientsController : Controller
    {
        private readonly IIngredientRepository _repository;

        public AdminIngredientsController(IIngredientRepository repo)
        {
            _repository = repo;
        }

        public IActionResult Index()
        {
            var model = _repository.Ingredients;

            return View(model);
        }

        public IActionResult AddIngredient(Produkt ingredient)
        {
            return View("EditOrAddIngredient", ingredient);
        }

        public IActionResult DeleteIngredient(int id)
        {
            _repository.DeleteIngredient(id);

            return RedirectToAction("Index");
        }

        public IActionResult EditIngredient(int id)
        {
            var model = _repository.GetSingleIngredient(id);

            return View("EditOrAddIngredient", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditOrAddIngredient(Produkt ingredient)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveIngredient(ingredient);

                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult AddIngredient()
        {
            return View("EditOrAddIngredient");
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
            if (_repository.CheckUniqueValue(ingredient.ProduktNamn) == false)
            {
                ModelState.AddModelError("ProduktNamn", "Name must be unique");
            }

            if (ModelState.IsValid)
            {
                _repository.SaveIngredient(ingredient);

                return RedirectToAction("Index");
            }

            return View(ingredient);
        }
    }
}
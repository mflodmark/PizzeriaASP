using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult EditOrAddIngredient(Produkt ingredient)
        {
            _repository.SaveIngredient(ingredient);

            return RedirectToAction("Index");

        }
    }
}
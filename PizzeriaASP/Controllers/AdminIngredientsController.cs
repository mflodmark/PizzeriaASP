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
        private readonly IProductRepository _repository;

        public AdminIngredientsController(IProductRepository repo)
        {
            _repository = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddIngredient(Produkt ingredient)
        {
            _repository

            return View("EditOrAddIngredient", model);
        }

        public IActionResult EditOrAddIngredient()
        {
            return View();

        }
    }
}
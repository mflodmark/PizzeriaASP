using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Components
{
    public class FilteringMenuViewComponent: ViewComponent
    {
        private IProductRepository repository;

        public FilteringMenuViewComponent(IProductRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            string currentCategory;
            currentCategory = 
                RouteData?.Values["category"] == null ? "" : RouteData?.Values["category"].ToString();

            var model = new CategoryViewModel()
            {
               Category = repository.Categories
                            .Select(x => x.Beskrivning)
                            .Distinct()
                            .OrderBy(x => x),
               CurrentCategory = currentCategory
            };

            return View(model);
        }
    }
}

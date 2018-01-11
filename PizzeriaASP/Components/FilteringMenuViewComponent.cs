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
        //private readonly IProductRepository repository;
        private readonly TomasosContext _context;


        //public FilteringMenuViewComponent(IProductRepository repo)
        //{
        //    repository = repo;
        //}
        public FilteringMenuViewComponent(TomasosContext context)
        {
            _context = context;
        }


        public IViewComponentResult Invoke()
        {
            var currentCategory = 
                RouteData?.Values["category"] == null ? "" : RouteData?.Values["category"].ToString();

            var model = new CategoryViewModel()
            {
               Category = _context.MatrattTyp
                            .Select(x => x.Beskrivning)
                            .Distinct()
                            .OrderBy(x => x),
               CurrentCategory = currentCategory
            };

            return View(model);
        }
    }
}

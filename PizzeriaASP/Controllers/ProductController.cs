using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    //[Authorize]
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public int PageSize = 4;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int productPage = 1) 
            => View(new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => p.MatrattTypNavigation.Beskrivning == category || category == null)
                    .OrderBy(p => p.MatrattNamn)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ? 
                        repository.Products.Count() : 
                        repository.Products.Count(x => 
                            x.MatrattTypNavigation.Beskrivning == category)
                },
                CurrentCategory = category
            });
    }
}
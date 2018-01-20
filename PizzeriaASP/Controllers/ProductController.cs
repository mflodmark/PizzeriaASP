using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;
using SQLitePCL;

namespace PizzeriaASP.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public int PageSize = 4;

        public ProductController(IProductRepository context)
        {
            _productRepository = context;
        }

        public IActionResult List(string category, int productPage = 1)
        {
            var products = _productRepository.Products
                .Where(p => p.MatrattTypNavigation.Beskrivning == category || category == null)
                .OrderBy(p => p.MatrattNamn)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize)
                .Include(x => x.MatrattProdukt)
                .ThenInclude(p => p.Produkt);
            
            var model = new ProductsListViewModel
            {
                Products = products,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                        _productRepository.Products.Count() :
                        _productRepository.Products.Count(x =>
                            x.MatrattTypNavigation.Beskrivning == category)
                },
                CurrentCategory = category,
            };

            return View(model);
        }
    }
}
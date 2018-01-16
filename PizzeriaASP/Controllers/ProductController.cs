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
        //private readonly IProductRepository _repository;

        public int PageSize = 4;
        private readonly TomasosContext _context;

        //public ProductController(IProductRepository repo)
        //{
        //    _repository = repo;
        //}


        public ProductController(TomasosContext context)
        {
            _context = context;
        }

        public IActionResult List(string category, int productPage = 1)
        {
            var ingredients = _context.Produkt.ToList();

            var products = _context.Matratt
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
                        _context.Matratt.Count() :
                        _context.Matratt.Count(x =>
                            x.MatrattTypNavigation.Beskrivning == category)
                },
                CurrentCategory = category,
            };

            return View(model);
        }
    }
}
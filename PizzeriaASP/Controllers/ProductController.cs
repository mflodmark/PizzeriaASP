using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<ApplicationUser> _userManager;


        public int PageSize = 4;

        public ProductController(IProductRepository context, ICustomerRepository customerRepository,
        UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _productRepository = context;
            _customerRepository = customerRepository;
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
                Customer = _customerRepository.GetSingleCustomer(_userManager.GetUserName(User))
        };

            return View(model);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            var model = Products(category, productPage);
           
            if (category == null) {return View(model);}

            RouteData.Values["category"] = category;
            
            return PartialView("_ProductList", model);
        }

        public IActionResult ProductList(string category, int productPage = 1)
        {
            var model = Products(category, productPage);

            RouteData.Values["category"] = category;

            return View("List", model);
        }

        private ProductsListViewModel Products(string category, int productPage = 1)
        {
            var products = _productRepository.Products
                .Where(p => p.MatrattTypNavigation.Beskrivning == category || category == null || category == "All")
                .OrderBy(p => p.MatrattNamn)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize)
                .Include(x => x.MatrattProdukt)
                .ThenInclude(p => p.Produkt);

            var customer = _customerRepository.GetSingleCustomer(_userManager.GetUserName(User));

            var totalItems = category == null || category == "All"
                ? _productRepository.Products.Count()
                : _productRepository.Products.Count(x =>
                    x.MatrattTypNavigation.Beskrivning == category);

            var pages = totalItems / PageSize + 1;

            var model = new ProductsListViewModel
            {
                Products = products,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null || category == "All" ?
                        _productRepository.Products.Count() :
                        _productRepository.Products.Count(x =>
                            x.MatrattTypNavigation.Beskrivning == category)
                },
                CurrentCategory = category,
                Customer = customer,
                OrderItems = GetCart().BestallningMatratt.Count,
                Categories = _productRepository.Categories,
                Pages = pages,
                CurrentPage = productPage

            };

            return model;
        }

        private Bestallning GetCart()
        {
            Bestallning order;
            if (HttpContext.Session.GetString("Varukorg") == null)
            {
                order = new Bestallning();
            }
            else
            {
                var serializedValue = HttpContext.Session.GetString("Varukorg");
                order = JsonConvert.DeserializeObject<Bestallning>(serializedValue);
            }

            return order;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaASP.Infrastructure;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    public class CartController : Controller
    {
        //private readonly IProductRepository _repository;
        //private readonly Bestallning _cart;
        private readonly TomasosContext _context;


        //public CartController(IProductRepository repo, Bestallning cartService)
        //{
        //    _repository = repo;
        //    _cart = cartService;
        //}

        public CartController(TomasosContext context)
        {
            _context = context;
        }

        public ViewResult Index(string returnUrl)
        {
            // Get session value
            var prodList = GetCart();

            return View(new CartIndexViewModel()
            {
                Orders = prodList,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {

            var product = _context.Matratt
                .FirstOrDefault(p => p.MatrattId == productId);

            var newProd = new BestallningMatratt()
            {
                Antal = 1, Matratt = product, MatrattId = productId
            };

            var prodList = GetCart();

            prodList.Add(newProd); 

            SetCart(prodList);

            return RedirectToAction("Index", new { returnUrl });

            //var product = _repository.Products
            //    .FirstOrDefault(p => p.MatrattId == productId);

            //if (product != null)
            //{
            //    _cart.AddItem(product, 1);
            //}

            //return RedirectToAction("Index", new { returnUrl });
        }

        private List<BestallningMatratt> GetCart()
        {
            List<BestallningMatratt> prodList;
            if (HttpContext.Session.GetString("Varukorg") == null)
            {
                prodList = new List<BestallningMatratt>();
            }
            else
            {
                var serializedValue = HttpContext.Session.GetString("Varukorg");
                prodList = JsonConvert.DeserializeObject<List<BestallningMatratt>>(serializedValue);
            }
            
            return prodList;
        }

        private void SetCart(List<BestallningMatratt> newList)
        {
            var temp = JsonConvert.SerializeObject(newList);
            HttpContext.Session.SetString("Varukorg", temp);
        }

        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        //public RedirectToActionResult AddToCart(int matrattId, string returnUrl)
        //{
        //    ICollection<BestallningMatratt> prodList;

        //    //Hämta produkten som objekt med hjälp av id (oftast från databasen) 
        //    var product = repository.Products.SingleOrDefault(p => p.MatrattId == matrattId);


        //    //OM det är första gången finns ingen varukorg
        //    if (HttpContext.Session.GetString("Varukorg") == null)
        //    {
        //        prodList = new List<BestallningMatratt>();
        //    }
        //    else
        //    {
        //        //Om det redan finns en varukorg. Hämta listan från sessionsvariabeln
        //        var serializedValue = (HttpContext.Session.GetString("Varukorg"));
        //        prodList = JsonConvert.DeserializeObject<List<BestallningMatratt>>(serializedValue);

        //    }

        //    var pl = new BestallningMatratt()
        //    {
        //        Antal = 1,

        //    };

        //    prodList.Add(product);

        //    cart.BestallningMatratt = prodList;

        //    //Lägga tillbaka listan i sessionsvariabeln
        //    var temp = JsonConvert.SerializeObject(prodList);
        //    HttpContext.Session.SetString("Varukorg", temp);

        //    return RedirectToAction("Index", new { returnUrl });
        //}

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = _context.Matratt.FirstOrDefault(p => p.MatrattId == productId);

            if (product != null)
            {
                var cart = GetCart();

                cart.RemoveAll(p => p.Matratt.MatrattId == product.MatrattId);

                SetCart(cart);
                //_cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

    }
}
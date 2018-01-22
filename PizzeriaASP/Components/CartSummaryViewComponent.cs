using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;
using Microsoft.AspNetCore.Identity;


namespace PizzeriaASP.Components
{
    public class CartSummaryViewComponent: ViewComponent
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityRepository _identityRepo;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public CartSummaryViewComponent(UserManager<ApplicationUser> userManager, ICustomerRepository customerRepository,
            IIdentityRepository identityRepo, SignInManager<ApplicationUser> signInManager)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _identityRepo = identityRepo;
            _signInManager = signInManager;
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

        public IViewComponentResult Invoke()
        {
            var cart = GetCart();

            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var userRole = GetUserRole().Result;

                var points = GetPoints().Result;

                var model = new CartIndexViewModel()
                {
                    Cart = cart,
                    CartTotalValue = cart.ComputeTotalValue(userRole, points, cart.BestallningMatratt.Sum(p => p.Antal))
                };

                return View(model);
            }

            var model2 = new CartIndexViewModel()
            {
                Cart = cart,
                CartTotalValue = 0
            };

            return View(model2);
        }

        private async Task<string> GetUserRole()
        {
            // Get user & role
            var user = await _userManager.GetUserAsync(HttpContext.User);
            
            var getUser = _identityRepo.GetSingleUser(user.UserName);

            var userRoles = _userManager.GetRolesAsync(getUser).Result;

            return userRoles[0];
        }

        private async Task<int> GetPoints()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            return _customerRepository.GetSingleCustomer(user.UserName).Poang;
        }
    }
}

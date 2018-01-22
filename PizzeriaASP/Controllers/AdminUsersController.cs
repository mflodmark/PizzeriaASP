using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzeriaASP.Models;
using PizzeriaASP.ViewModels;

namespace PizzeriaASP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IIdentityRepository _identityRepo;
        private readonly ICustomerRepository _customerRepository;

        //private readonly TomasosContext _tomasosContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminUsersController(IIdentityRepository identityRepo, UserManager<ApplicationUser> userManager,
            ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            _identityRepo = identityRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = GetIndexData();

            return View(model);
        }

        public IActionResult EditRole(string user)
        {
            var getUser = _identityRepo.GetSingleUser(user);

            var userRoles = _userManager.GetRolesAsync(getUser).Result;

            var role = userRoles.Count == 0 ? "Not set" : userRoles[0];

            var roles = _identityRepo.Roles.Select(p => new SelectListItem()
            {
                Value = p.Id,
                Text = p.Name
            }).OrderBy(o => o.Text).ToList();

            // Remove admin role from selectable list
            var adminRole = roles.Single(x => x.Text == "Admin");

            roles.Remove(adminRole);

            var model = new AdminUsersEditRoleViewModel()
            {
                Roles = roles,
                Role = role,
                User = getUser
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(AdminUsersEditRoleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = _identityRepo.GetSingleUser(vm.User.UserName);

                // Get roles
                var roles = await _userManager.GetRolesAsync(user);
                
                // Remove roles
                foreach (var item in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }

                var role = _identityRepo.GetSingleRole(vm.SelectedRole);

                // Set new role
                await _userManager.AddToRoleAsync(user, role.Name);

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> DeleteUser(string username)
        {
            var user = _identityRepo.GetSingleUser(username);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    _customerRepository.DeleteCustomer(username);

                }
            }

            var model = GetIndexData();

            return PartialView("_DeleteUserPartial", model);

        }

        private List<AdminUsersIndexViewModel> GetIndexData()
        {
            var model = new List<AdminUsersIndexViewModel>();
            var users = _identityRepo.Users.OrderBy(u => u.UserName);

            foreach (var item in users)
            {
                var userRoles = _userManager.GetRolesAsync(item).Result;

                var role = userRoles.Count == 0 ? "Not set" : userRoles[0];

                model.Add(new AdminUsersIndexViewModel()
                {
                    User = item,
                    Role = role,
                });
            }

            return model;
        }
    }
}
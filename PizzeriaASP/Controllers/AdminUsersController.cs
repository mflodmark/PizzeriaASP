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
        private readonly ApplicationDbContext _context;
        private readonly TomasosContext _tomasosContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            TomasosContext tomasosContext)
        {
            _tomasosContext = tomasosContext;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var model = new List<AdminUsersIndexViewModel>();
            var users = _context.Users.OrderBy(u => u.UserName);

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

            return View(model);
        }

        public IActionResult EditRole(string user)
        {
            var getUser = _context.Users.Single(x => x.UserName == user);

            var userRoles = _userManager.GetRolesAsync(getUser).Result;

            var role = userRoles.Count == 0 ? "Not set" : userRoles[0];

            var roles = _context.Roles.Distinct().Select(p => new SelectListItem()
            {
                Value = p.Id,
                Text = p.Name
            }).OrderBy(o => o.Text).ToList();

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
                var user = _context.Users.Single(x => x.UserName == vm.User.UserName);

                // Get roles
                var roles = await _userManager.GetRolesAsync(user);


                // Remove roles
                foreach (var item in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }

                var role = _context.Roles.Find(vm.SelectedRole);

                // Set new role
                await _userManager.AddToRoleAsync(user, role.Name);

                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = _context.Users.Single(x => x.UserName == username);

            //var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    // Delete from customer as well
                    var customer = _tomasosContext.Kund.Single(x => x.AnvandarNamn == username);

                    var orders = _tomasosContext.Bestallning.Where(x => x.KundId == customer.KundId);

                    foreach (var order in orders)
                    {
                        var orderItems =
                            _tomasosContext.BestallningMatratt
                            .Where(x => x.BestallningId == order.BestallningId);

                        // Delete order items
                        foreach (var orderItem in orderItems)
                        {
                            _tomasosContext.BestallningMatratt.Remove(orderItem);
                        }

                        _tomasosContext.SaveChanges();

                        // Delete orders
                        _tomasosContext.Bestallning.Remove(order);

                        _tomasosContext.SaveChanges();

                    }

                    // Delete customer
                    _tomasosContext.Kund.Remove(customer);

                    _tomasosContext.SaveChanges();
                }
            }

            return RedirectToAction("Index");

        }

    }
}
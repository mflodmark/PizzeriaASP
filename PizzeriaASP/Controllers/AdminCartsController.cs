using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PizzeriaASP.Controllers
{
    public class AdminCartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzeriaASP.Models;

namespace PizzeriaASP.ViewModels
{
    public class AdminIndexViewModel
    {
        public Matratt Product { get; set; }

        public string ProductType { get; set; }
    }
}
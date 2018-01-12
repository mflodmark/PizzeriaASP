﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzeriaASP.Models;

namespace PizzeriaASP.ViewModels
{
    public class AdminEditViewModel
    {
        public Matratt Product { get; set; }

        public List<SelectListItem> ProductTypes { get; set; }
    }
}
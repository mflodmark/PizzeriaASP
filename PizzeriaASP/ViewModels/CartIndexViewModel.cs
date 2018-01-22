using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzeriaASP.Models;

namespace PizzeriaASP.ViewModels
{
    public class CartIndexViewModel
    {
        public Bestallning Cart { get; set; }

        public int CartTotalValue { get; set; }

        public double CartRebate { get; set; }

        public int Points { get; set; }

        public string ReturnUrl { get; set; }
    }
}

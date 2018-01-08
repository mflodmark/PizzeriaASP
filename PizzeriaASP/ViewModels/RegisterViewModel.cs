using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzeriaASP.Models;

namespace PizzeriaASP.ViewModels
{
    public class RegisterViewModel
    {
        public Kund Customer { get; set; }

        public bool UniqueUsername { get; set; }
    }
}

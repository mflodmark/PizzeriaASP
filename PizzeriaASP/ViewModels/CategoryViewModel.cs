using System.Collections.Generic;

namespace PizzeriaASP.ViewModels
{
    public class CategoryViewModel
    {
        public string CurrentCategory { get; set; }

        public IEnumerable<string> Category { get; set; }
    }
}
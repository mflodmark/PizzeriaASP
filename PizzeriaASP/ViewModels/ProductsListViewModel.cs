using System.Collections.Generic;
using PizzeriaASP.Models;

namespace PizzeriaASP.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Matratt> Products { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}
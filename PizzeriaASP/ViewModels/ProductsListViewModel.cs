using System.Collections.Generic;
using PizzeriaASP.Models;

namespace PizzeriaASP.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<Matratt> Products { get; set; }

        public IEnumerable<MatrattTyp> Categories { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string CurrentCategory { get; set; }

        public Kund Customer { get; set; }

        public int OrderItems { get; set; }

        public int Pages { get; set; }

        public int CurrentPage { get; set; }

    }
}
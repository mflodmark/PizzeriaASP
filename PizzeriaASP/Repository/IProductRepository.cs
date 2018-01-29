using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PizzeriaASP.Models
{
    public interface IProductRepository
    {
        IQueryable<Matratt> Products { get; }

        IQueryable<MatrattTyp> Categories { get; }

        IQueryable<Produkt> Ingredients { get; }

        IQueryable<MatrattProdukt> ProductIngridientList { get; }


        Matratt GetSingleProduct(int id);

        Produkt GetSingleIngredient(int id);

        void DeleteProduct(int id);
       
        void SaveProduct(Matratt product);

        void SaveIngredientList(int id, List<Produkt> productList);
                
        List<Produkt> GetCurrentIngredients(int id);

        List<SelectListItem> GetProductTypes();

        List<Produkt> GetOptionalIngredients(int id, List<Produkt> producList);

        List<Produkt> GetAllIngredients();

        bool CheckUniqueValue(string name);


    }
}

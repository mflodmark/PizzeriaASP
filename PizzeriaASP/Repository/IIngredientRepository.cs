﻿using System.Linq;

namespace PizzeriaASP.Models
{
    public interface IIngredientRepository
    {
        IQueryable<Produkt> Ingredients { get; }

        void DeleteIngredient(int id);

        void SaveIngredient(Produkt ingredient);

        Produkt GetSingleIngredient(int id);

        bool CheckUniqueValue(string name);
    }
}

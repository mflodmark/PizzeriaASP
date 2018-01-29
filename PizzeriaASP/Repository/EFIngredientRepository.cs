using System.Linq;
using Microsoft.EntityFrameworkCore;
using PizzeriaASP.Models;

namespace PizzeriaASP
{

    public class EFIngredientRepository : IIngredientRepository
    {
        private readonly TomasosContext _context;

        public EFIngredientRepository(TomasosContext context)
        {
            _context = context;
        }

        public IQueryable<Produkt> Ingredients => _context.Produkt
            .Include(x=>x.MatrattProdukt)
            .ThenInclude(x=>x.Produkt);

        public Produkt GetSingleIngredient(int id)
        {
            return _context.Produkt.Single(p => p.ProduktId == id);
        }

        public bool CheckUniqueValue(string name)
        {
            if (_context.Produkt.Any(p => p.ProduktNamn == name))
            {
                return false;
            }

            return true;
        }


        public void DeleteIngredient(int id)
        {
            _context.Produkt.Remove(GetSingleIngredient(id));

            _context.SaveChanges();
        }

        public void SaveIngredient(Produkt ingredient)
        {
            if (ingredient.ProduktId == 0)
            {
                // Add new product
                _context.Produkt.Add(ingredient);
            }
            else
            {
                // Edit existing product
                var p = _context.Produkt.FirstOrDefault(x => x.ProduktId == ingredient.ProduktId);

                if (p != null)
                {
                    _context.Entry(p).CurrentValues.SetValues(ingredient);
                }
            }
            _context.SaveChanges();
        }


    }
}
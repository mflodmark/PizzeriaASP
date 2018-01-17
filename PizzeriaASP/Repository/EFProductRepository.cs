using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class EFProductRepository : IProductRepository
    {
        private readonly TomasosContext _context;

        public EFProductRepository(TomasosContext context)
        {
            _context = context;
        }

        public IQueryable<Matratt> Products => _context.Matratt;

        public IQueryable<MatrattTyp> Categories => _context.MatrattTyp;

        public IQueryable<Produkt> Ingredients => _context.Produkt;
        public IQueryable<MatrattProdukt> ProductIngridientList { get; }

        public Matratt GetSingleProduct(int id)
        {
            throw new System.NotImplementedException();
        }

        public Produkt GetSingleIngredient(int id)
        {
            return _context.Produkt.Single(p => p.ProduktId == id);
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Matratt.Find(id);

            _context.Remove(product);

            _context.SaveChanges();
            _context.Dispose();
        }

        public void DeleteIngredient(int id)
        {
            throw new System.NotImplementedException();
        }

        public void SaveProduct(Matratt product)
        {
            if(product.MatrattId == 0)
            {
                // Add new product
                _context.Matratt.Add(product);
            } else
            {
                // Edit existing product
                var p = _context.Matratt.FirstOrDefault(x => x.MatrattId == product.MatrattId);

                if (p != null)
                {
                    _context.Entry(p).CurrentValues.SetValues(product);
                }
            }
            _context.SaveChanges();
            _context.Dispose();
        }

        public void SaveIngredient(Produkt ingredient)
        {
            throw new System.NotImplementedException();
        }

        public List<Produkt> GetIngredients(int id)
        {
            var i = _context.MatrattProdukt
                .Where(x => x.MatrattId == id)
                .Select(y => y.Produkt)
                .ToList();

            return i;
        }

        public List<SelectListItem> GetProductTypes()
        {
            return _context.MatrattTyp.Select(p => new SelectListItem()
            {
                Value = p.MatrattTyp1.ToString(),
                Text = p.Beskrivning
            }).OrderBy(o => o.Text).ToList();
        }

        public List<SelectListItem> GetIngredients()
        {
            return _context.Produkt.Select(p => new SelectListItem()
            {
                Value = p.ProduktId.ToString(),
                Text = p.ProduktNamn
            }).OrderBy(o => o.Text).ToList();
        }
    }



}
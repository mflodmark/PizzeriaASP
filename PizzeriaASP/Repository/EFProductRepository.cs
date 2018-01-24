using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public IQueryable<Matratt> Products =>
            _context.Matratt
            .Include(p => p.BestallningMatratt)
            .ThenInclude(p => p.Matratt)
            .Include(p=>p.MatrattProdukt)
            .ThenInclude(p=>p.Matratt);

        public IQueryable<MatrattTyp> Categories => _context.MatrattTyp
            .Distinct()
            .OrderBy(x => x.Beskrivning);


        public IQueryable<Produkt> Ingredients => _context.Produkt;

        public IQueryable<MatrattProdukt> ProductIngridientList => _context.MatrattProdukt;

        public Matratt GetSingleProduct(int id)
        {
            return _context.Matratt.SingleOrDefault(p => p.MatrattId == id);
        }

        public Produkt GetSingleIngredient(int id)
        {
            return _context.Produkt.SingleOrDefault(p => p.ProduktId == id);
        }

        public void DeleteProduct(int id)
        {
            var product = Products.SingleOrDefault(x=> x.MatrattId == id);

            foreach (var ingredient in product.MatrattProdukt)
            {
                _context.MatrattProdukt.Remove(ingredient);
            }

            _context.SaveChanges();

            _context.Remove(product);

            _context.SaveChanges();
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
        }

        public void SaveIngredientList(int id, List<Produkt> productList)
        {
            foreach (var item in productList)
            {
                if (_context.MatrattProdukt.SingleOrDefault(p => 
                p.ProduktId == item.ProduktId && p.MatrattId == id) == null)
                {
                    _context.MatrattProdukt.Add(new MatrattProdukt()
                    {
                        MatrattId = id,
                        ProduktId = item.ProduktId
                    });
                }
            }

            _context.SaveChanges();
        }


        public List<Produkt> GetCurrentIngredients(int id)
        {
            if (id == 0)
            {
                var i = _context.MatrattProdukt
                    .Where(x => x.MatrattId == id)
                    .Select(y => y.Produkt)
                    .ToList();

                return i;
            }

            return new List<Produkt>();
        }

        public List<SelectListItem> GetProductTypes()
        {
            return _context.MatrattTyp.Select(p => new SelectListItem()
            {
                Value = p.MatrattTyp1.ToString(),
                Text = p.Beskrivning
            }).OrderBy(o => o.Text).ToList();
        }

        public List<Produkt> GetOptionalIngredients(int id, List<Produkt> productList)
        {
            var currentList = productList;

            var optionalList = _context.Produkt.OrderBy(o => o.ProduktNamn);

            var list = optionalList.Where(x => x.ProduktId != 
                currentList.FirstOrDefault(y => y.ProduktId == x.ProduktId).ProduktId).ToList();

            return list;
        }

        public List<Produkt> GetAllIngredients()
        {
            return _context.Produkt.OrderBy(x => x.ProduktNamn).ToList();
        }
    }



}
using System.Linq;
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

        public void SaveProduct(Matratt product)
        {
            if(product.MatrattId == 0)
            {
                _context.Matratt.Add(product);
            } else
            {
                var db = _context.Matratt.FirstOrDefault(x => x.MatrattId == product.MatrattId);

                if(db != null)
                {
                    db.MatrattNamn = product.MatrattNamn;
                    db.Beskrivning = product.Beskrivning;
                    db.Pris = product.Pris;
                    
                }
            }
            _context.SaveChanges();

            _context.Dispose();
        }
    }



}
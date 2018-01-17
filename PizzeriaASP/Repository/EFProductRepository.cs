using System.Linq;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class EFProductRepository : IProductRepository
    {
        private readonly TomasosContext _context;
        private readonly IProductRepository _productRepository;

        public EFProductRepository(TomasosContext context, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public IQueryable<Matratt> Products => _context.Matratt;

        public IQueryable<MatrattTyp> Categories => _context.MatrattTyp;

        public void RemoveProduct(Matratt product)
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

                if(p != null)
                {
                    //db.MatrattNamn = product.MatrattNamn;
                    //db.Beskrivning = product.Beskrivning;
                    //db.Pris = product.Pris;
                    _context.Entry(p).CurrentValues.SetValues(product);
                }
            }
            _context.SaveChanges();

            _context.Dispose();
        }
    }



}
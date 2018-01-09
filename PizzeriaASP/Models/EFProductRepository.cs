using System.Linq;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class EfProductRepository : IProductRepository
    {
        private TomasosContext _context;

        public EfProductRepository(TomasosContext context)
        {
            _context = context;
        }

        public IQueryable<Matratt> Products => _context.Matratt;

        public IQueryable<MatrattTyp> Categories => _context.MatrattTyp;

    }
}
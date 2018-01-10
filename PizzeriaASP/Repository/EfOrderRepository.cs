using System.Linq;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class EFOrderRepository : IOrderRepository
    {
        private TomasosContext _context;

        public EFOrderRepository(TomasosContext context)
        {
            _context = context;
        }

        public IQueryable<Bestallning> Orders => _context.Bestallning;

        public void SaveOrder(Bestallning order)
        {
            // Ensures that 
            _context.AttachRange(order.Lines.Select(l => l.Matratt));

            if (order.BestallningId == 0)
            {
                _context.Bestallning.Add(order);
            }

            _context.SaveChanges();

            _context.Dispose();
        }


    }



}
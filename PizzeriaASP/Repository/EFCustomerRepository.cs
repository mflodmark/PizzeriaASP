using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class EFCustomerRepository : ICustomerRepository
    {
        private readonly TomasosContext _context;

        public EFCustomerRepository(TomasosContext context)
        {
            _context = context;
        }


        public IQueryable<Kund> Customers => _context.Kund
            .Include(p => p.Bestallning)
            .ThenInclude(p => p.BestallningMatratt);

        public void SaveCustomer(Kund customer)
        {
            if (customer.KundId == 0)
            {
                // Add new product
                _context.Kund.Add(customer);
            }
            else
            {
                // Edit existing product
                var p = Customers.FirstOrDefault(x => x.KundId == customer.KundId);

                if (p != null)
                {
                    _context.Entry(p).CurrentValues.SetValues(customer);
                }
            }
            _context.SaveChanges();
        }

        public void DeleteCustomer(string username)
        {
            var customer = GetSingleCustomer(username);

            // Delete order items
            foreach (var order in customer.Bestallning)
            {
                foreach (var item in order.BestallningMatratt)
                {
                    // Delete orders order items
                    _context.BestallningMatratt.Remove(item);
                }
                _context.SaveChanges();

                // Delete order
                _context.Bestallning.Remove(order);

            }

            _context.SaveChanges();

            // Delete customer
            _context.Kund.Remove(customer);

            _context.SaveChanges();

        }

        public Kund GetSingleCustomer(string username)
        {
            return Customers.Single(p => p.AnvandarNamn == username);
        }

        public List<Kund> GetOrdersForCustomer(int id)
        {
            return Customers.Where(p => p.KundId == id).ToList();
        }
    }

}
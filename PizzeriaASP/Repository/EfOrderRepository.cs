﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PizzeriaASP.Models;

namespace PizzeriaASP
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly TomasosContext _context;

        public EFOrderRepository(TomasosContext context)
        {
            _context = context;
        }


        public IQueryable<Bestallning> Orders => _context.Bestallning
            .Include(p => p.BestallningMatratt)
            .ThenInclude(p => p.Matratt)
            .Include(p => p.Kund);

        public void SaveOrder(Bestallning order)
        {
            if (order.BestallningId == 0)
            {
                // Add new product
                _context.Bestallning.Add(order);
            }
            else
            {
                // Edit existing product
                var p = Orders.FirstOrDefault(x => x.BestallningId == order.BestallningId);

                if (p != null)
                {
                    _context.Entry(p).CurrentValues.SetValues(order);
                }
            }

            _context.SaveChanges();

            foreach (var c in order.BestallningMatratt)
            {
                _context.BestallningMatratt.Add(c);
            }

            _context.SaveChanges();
        }

        public void UpdateDeliveryStatus(int id, bool status)
        {
            var order = GetSingleOrder(id);

            order.Levererad = status;

            _context.SaveChanges();
        }


        public void DeleteOrder(int id)
        {
            var order = GetSingleOrder(id);

            // Delete order items
            foreach (var orderItem in order.BestallningMatratt)
            {
                _context.BestallningMatratt.Remove(orderItem);
            }

            _context.SaveChanges();

            // Delete orders
            _context.Bestallning.Remove(order);

            _context.SaveChanges();
        }

        public Bestallning GetSingleOrder(int id)
        {
            return Orders.Single(p => p.BestallningId == id);
        }

        public List<Bestallning> GetOrdersForCustomer(int id)
        {
            return Orders.Where(p => p.KundId == id).ToList();
        }
    }

}
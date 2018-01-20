using System.Collections.Generic;
using System.Linq;

namespace PizzeriaASP.Models
{
    public interface IOrderRepository
    {
        IQueryable<Bestallning> Orders { get; }

        void SaveOrder(Bestallning order);

        void UpdateDeliveryStatus(int id, bool status);
        
        void DeleteOrder(int id);

        Bestallning GetSingleOrder(int id);

        List<Bestallning> GetOrdersForCustomer(int id);
    }
}

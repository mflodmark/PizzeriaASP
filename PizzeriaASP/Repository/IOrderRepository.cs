using System.Linq;

namespace PizzeriaASP.Models
{
    public interface IOrderRepository
    {
        IQueryable<Bestallning> Orders { get; }

        void SaveOrder(Bestallning order);
    }
}

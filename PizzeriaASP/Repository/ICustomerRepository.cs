using System.Linq;

namespace PizzeriaASP.Models
{
    public interface ICustomerRepository
    {
        IQueryable<Kund> Customers { get; }

        void SaveCustomer(Kund customer);

        void DeleteCustomer(string username);

        Kund GetSingleCustomer(string username);

    }
}

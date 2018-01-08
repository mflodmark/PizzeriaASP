using System;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaASP.Models
{
    public interface IProductRepository
    {
        IQueryable<Matratt> Products { get; }
    }
}

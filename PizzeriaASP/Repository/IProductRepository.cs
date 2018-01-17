using System;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaASP.Models
{
    public interface IProductRepository
    {
        IQueryable<Matratt> Products { get; }

        IQueryable<MatrattTyp> Categories { get; }
    
        void RemoveProduct(Matratt product);

        void SaveProduct(Matratt product);


    }
}

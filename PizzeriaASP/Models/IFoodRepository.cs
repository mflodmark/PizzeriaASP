using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaASP.Models
{
    public interface IFoodRepository
    {
        IQueryable<Matratt> Food { get; }
    }
}

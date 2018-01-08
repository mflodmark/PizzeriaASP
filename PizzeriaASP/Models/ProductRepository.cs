using System.Collections.Generic;
using System.Linq;

namespace PizzeriaASP.Models
{
    public class ProductRepository: IProductRepository
    {
        public IQueryable<Matratt> Products => new List<Matratt>
        {
            new Matratt() {Beskrivning = "Vesuvio beskrivning", MatrattNamn = "Vesuvio", Pris = 99}
        }.AsQueryable<Matratt>();
    }
}
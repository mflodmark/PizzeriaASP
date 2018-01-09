using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace PizzeriaASP.Models
{
    public partial class Bestallning
    {
        public Bestallning()
        {
            BestallningMatratt = new HashSet<BestallningMatratt>();
        }

        public int BestallningId { get; set; }

        public DateTime BestallningDatum { get; set; }

        public int Totalbelopp { get; set; }

        public bool Levererad { get; set; }

        public int KundId { get; set; }

        public Kund Kund { get; set; }

        public ICollection<BestallningMatratt> BestallningMatratt { get; set; } 

        public virtual void AddItem(Matratt product, int quantity)
        {
            var line = BestallningMatratt.FirstOrDefault(p => p.Matratt.MatrattId == product.MatrattId);

            if (line == null)
            {
                BestallningMatratt.Add(new BestallningMatratt()
                {
                    Antal = quantity,
                    Matratt = product
                });
            }
            else
            {
                line.Antal += quantity;
            }
        }

        public virtual decimal ComputeTotalValue() => 
            BestallningMatratt.Sum(e => e.Matratt.Pris * e.Antal);

        public virtual void Clear() => BestallningMatratt.Clear();

        public virtual IEnumerable<BestallningMatratt> Lines => BestallningMatratt;

        //public virtual void RemoveLine(Matratt product) =>
        //    BestallningMatratt.Where(x => x.MatrattId == product.MatrattId);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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

            //List<BestallningMatratt> prodList;
            //BestallningMatratt newProd = new BestallningMatratt() {Antal = 1, Matratt = product};

            //if (HttpContext.Session.GetString("Varukorg") == null)
            //{
            //    prodList = new List<BestallningMatratt>();
            //}
            //else
            //{
            //    // Hämta listan från sessionen
            //    var serValue = HttpContext.Session.GetString("Varukorg");
            //    prodList = JsonConvert.DeserializeObject<List<BestallningMatratt>>(serValue);
            //}
            
            //prodList.Add(newProd);

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

        public virtual void RemoveLine(Matratt product)
        {
            BestallningMatratt.ToList().RemoveAll(p => p.Matratt.MatrattId == product.MatrattId);
        }

    }
}

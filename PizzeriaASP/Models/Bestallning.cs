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

        public int ComputeTotalValue(string role, int points)
        {
            var totalValue = BestallningMatratt.Sum(e => e.Matratt.Pris * e.Antal);

            if (role.ToLower().Contains("premium"))
            {
                var rebate = GetRebate(role);
                var totalValueIncRebate = Convert.ToInt32(totalValue * (1 - rebate));

                if (points >= 100)
                {
                    var cheapestPizzaForFree = BestallningMatratt.Min(p => p.Matratt.Pris);

                    var totalValueIncPoints = totalValueIncRebate - cheapestPizzaForFree;

                    if (totalValueIncPoints < 0)
                    {
                        return 0;
                    }
                    
                    return totalValueIncPoints;
                }

                return totalValueIncRebate;
            }

            return totalValue;
        }

        public double GetRebate(string role)
        {
            if (role.ToLower().Contains("premium"))
            {
                return 0.2;
            }
            return 0;
        }

    }
}

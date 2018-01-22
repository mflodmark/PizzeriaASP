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

        public int ComputeTotalValue(string role, int points, int orderItems)
        {
            var totalValue = BestallningMatratt.Sum(e => e.Matratt.Pris * e.Antal);

            if (role.ToLower().Contains("premium"))
            {
                return ComputeRebateValue(totalValue ,role, points, orderItems);
            }

            return totalValue;
        }

        public int ComputeRebateValue(int totalValue, string role, int points, int orderItems)
        {
            var rebate = GetRebate(role);

            if (points + orderItems * 10 >= 100 && BestallningMatratt.Sum(x => x.Antal) >= 3)
            {
                totalValue = (int) (totalValue * (1 - GetRebate(role)));

                var totalValueIncPoints = totalValue - ComputeRebateProduct(role);

                if (totalValueIncPoints < 0)
                {
                    return 0;
                }

                return totalValueIncPoints;
            }

            if (points + orderItems * 10 >= 100)
            {
                var totalValueIncPoints = totalValue - ComputeRebateProduct(role);

                var total = (int)(totalValueIncPoints * (1 - GetRebate(role)));

                if (total < 0)
                {
                    return 0;
                }

                return total;
            }

            if (BestallningMatratt.Sum(x => x.Antal) >= 3)
            {
                var totalValueIncRebate = Convert.ToInt32(totalValue * (1 - rebate));
                return totalValueIncRebate;
            }

            return totalValue;
        }


        public int ComputeRebateProduct(string role)
        {
            if (BestallningMatratt.Sum(x => x.Antal) >= 3)
            {
                var cheapestPizzaForFree = BestallningMatratt.Min(p => p.Matratt.Pris);

                return (int)(cheapestPizzaForFree * (1 - GetRebate(role)));
            }

            return BestallningMatratt.Min(p => p.Matratt.Pris);
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

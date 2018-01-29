using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaASP.Models
{
    public partial class Matratt
    {
        public Matratt()
        {
            BestallningMatratt = new HashSet<BestallningMatratt>();
            MatrattProdukt = new HashSet<MatrattProdukt>();
        }

        public int MatrattId { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Product name")]
        public string MatrattNamn { get; set; }

        [Required]
        [MaxLength(200)]
        [DisplayName("Description")]
        public string Beskrivning { get; set; }

        [Required]
        [Range(1, 10000)]
        [DisplayName("Price")]
        public int Pris { get; set; }

        [Required]
        public int MatrattTyp { get; set; }

        public MatrattTyp MatrattTypNavigation { get; set; }
        public ICollection<BestallningMatratt> BestallningMatratt { get; set; }
        public ICollection<MatrattProdukt> MatrattProdukt { get; set; }

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

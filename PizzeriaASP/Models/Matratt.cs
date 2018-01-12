using System;
using System.Collections.Generic;
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
        public string MatrattNamn { get; set; }

        [Required]
        [MaxLength(200)]
        public string Beskrivning { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public int Pris { get; set; }

        [Required]
        public int MatrattTyp { get; set; }

        public MatrattTyp MatrattTypNavigation { get; set; }
        public ICollection<BestallningMatratt> BestallningMatratt { get; set; }
        public ICollection<MatrattProdukt> MatrattProdukt { get; set; }
    }
}

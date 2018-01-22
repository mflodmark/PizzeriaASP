using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaASP.Models
{
    public partial class BestallningMatratt
    {
        public int MatrattId { get; set; }
        public int BestallningId { get; set; }

        [Range(1,int.MaxValue)]
        public int Antal { get; set; }

        public Bestallning Bestallning { get; set; }
        public Matratt Matratt { get; set; }
    }
}

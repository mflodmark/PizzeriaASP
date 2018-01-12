using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PizzeriaASP.Models
{
    public partial class MatrattTyp
    {
        public MatrattTyp()
        {
            Matratt = new HashSet<Matratt>();
        }

        public int MatrattTyp1 { get; set; }

        [DisplayName("Typ av maträtt")]
        public string Beskrivning { get; set; }

        public ICollection<Matratt> Matratt { get; set; }
    }
}

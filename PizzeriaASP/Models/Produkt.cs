using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaASP.Models
{
    public partial class Produkt
    {
        public Produkt()
        {
            MatrattProdukt = new HashSet<MatrattProdukt>();
        }

        public int ProduktId { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Ingredient Name")]
        public string ProduktNamn { get; set; }

        public ICollection<MatrattProdukt> MatrattProdukt { get; set; }
    }
}

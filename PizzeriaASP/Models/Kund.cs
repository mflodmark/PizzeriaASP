using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaASP.Models
{
    public partial class Kund
    {
        public Kund()
        {
            Bestallning = new HashSet<Bestallning>();
        }

        public int KundId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Namn { get; set; }

        [Required]
        [MaxLength(50)]
        public string Gatuadress { get; set; }

        [Required]
        [MaxLength(20)]
        public string Postnr { get; set; }

        [Required]
        [MaxLength(100)]
        public string Postort { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Telefon { get; set; }

        [Required]
        [DisplayName("Username")]
        [MaxLength(50)]
        public string AnvandarNamn { get; set; }

        [Required]
        [MaxLength(20)]
        public string Losenord { get; set; }

        public ICollection<Bestallning> Bestallning { get; set; }

        public int Poang { get; set; }
    }
}

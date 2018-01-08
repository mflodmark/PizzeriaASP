using System;
using System.Collections.Generic;
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

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string Namn { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string Gatuadress { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string Postnr { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string Postort { get; set; }

        public string Email { get; set; }

        public string Telefon { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string AnvandarNamn { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        public string Losenord { get; set; }

        public ICollection<Bestallning> Bestallning { get; set; }
    }
}

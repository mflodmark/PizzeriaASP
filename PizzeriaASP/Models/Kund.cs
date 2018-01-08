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
        public string Namn { get; set; }

        [Required]
        public string Gatuadress { get; set; }

        [Required]
        public string Postnr { get; set; }

        [Required]
        public string Postort { get; set; }

        public string Email { get; set; }

        public string Telefon { get; set; }

        [Required]
        [DisplayName("Username")]
        public string AnvandarNamn { get; set; }

        [Required]
        public string Losenord { get; set; }

        //public int UserId { get; set; }

        //public ApplicationUser User { get; set; }

        public ICollection<Bestallning> Bestallning { get; set; }
    }
}

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
        [DisplayName("Full name*")]
        public string Namn { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Streetaddress*")]
        public string Gatuadress { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Zipcode*")]
        public string Postnr { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("City*")]
        public string Postort { get; set; }

        [MaxLength(50)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [MaxLength(50)]
        [DisplayName("Telephone")]
        public string Telefon { get; set; }

        [Required]
        [DisplayName("Username*")]
        [MaxLength(50)]
        public string AnvandarNamn { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        [DisplayName("Password*")]
        public string Losenord { get; set; }

        public ICollection<Bestallning> Bestallning { get; set; }

        public int Poang { get; set; }

    }
}

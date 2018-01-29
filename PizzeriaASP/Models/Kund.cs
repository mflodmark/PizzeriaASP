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
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Only Alphabets and Numbers allowed for full name.")]
        public string Namn { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Streetaddress*")]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Only Alphabets and Numbers allowed for Streetaddress.")]
        public string Gatuadress { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Zipcode*")]
        [RegularExpression("^[0-9 ]*$", ErrorMessage = "Only Numbers allowed for Zipcode.")]
        public string Postnr { get; set; }

        [Required]
        [MaxLength(100)]
        [DisplayName("City*")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets allowed for City.")]
        public string Postort { get; set; }

        [MaxLength(50)]
        [DisplayName("Email")]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(50)]
        [DisplayName("Telephone")]
        [RegularExpression("^[0-9 -]*$", ErrorMessage = "Only Numbers allowed for Zipcode.")]
        public string Telefon { get; set; }

        [Required]
        [DisplayName("Username*")]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Only Alphabets and Numbers allowed for username.")]
        public string AnvandarNamn { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        [DisplayName("Password*")]
        [RegularExpression("^[a-z]+[A-Z]+[0-9]+[$@$!%*?&]+$", 
            ErrorMessage = "Password must be at least 6 characters and use a-z, A-Z and 0-9")]
        public string Losenord { get; set; }

        public ICollection<Bestallning> Bestallning { get; set; }

        public int Poang { get; set; }

    }
}

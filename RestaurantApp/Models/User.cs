using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public abstract class User
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string OwnerLastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string OwnerFirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{2}\d{2}[A-Z]{4}\d{16}$", ErrorMessage = "Invalid IBAN format.")]
        public string IBAN { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return OwnerLastName + ", " + OwnerFirstName;
            }
        }
    }
}

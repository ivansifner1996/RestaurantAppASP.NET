using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApp.Models
{
    public class Product
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Code")]
        public int ProductID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must have up to 2 decimal places")]
        public decimal Price { get; set; }

        [StringLength(50, ErrorMessage = "Description cannot exceed 50 characters.")] 
        public string Description { get; set; }

        public int MenuId { get; set; }

        public Menu Menu { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
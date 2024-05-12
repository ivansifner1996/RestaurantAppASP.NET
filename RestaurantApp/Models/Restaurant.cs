using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class Restaurant
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Foundation Date")]
        public DateTime FoundationDate { get; set; }

        [Required]
        public string Country { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        public List<Menu> Menus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public class Menu
    {
        public int MenuId { get; set; }

        [Required]
        [StringLength(20, MinimumLength=2)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
        
        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }

        public List<Product> Products { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.Models
{
    public enum Rating
    {
        A = 1, B, C, D ,E
    }
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductID { get; set; }

        public int CustomerID { get; set; }

        [DisplayFormat(NullDisplayText = "No rating provided")]
        public Rating? Rate { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
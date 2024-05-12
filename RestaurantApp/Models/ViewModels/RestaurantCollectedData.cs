using System.Collections.Generic;

namespace RestaurantApp.Models.ViewModels
{
    public class RestaurantCollectedData
    {
        public IEnumerable<Restaurant> Restaurants;
        public IEnumerable<Menu> Menus;
        public IEnumerable<Product> Products;

    }
}

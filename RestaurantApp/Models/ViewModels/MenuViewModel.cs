using System;
using System.Collections.Generic;

namespace RestaurantApp.Models.ViewModels
{
    public class MenuViewModel<T> : ViewWithDialogModel<Menu>
    {
        public List<Menu> menu { get; set; }
        //public Menu menu { get; set; }

        public dynamic? OtherData { get; set; }

    }
}

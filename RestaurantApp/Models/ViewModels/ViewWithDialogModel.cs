using System;
using Microsoft.AspNetCore.Html;

namespace RestaurantApp.Models.ViewModels
{

    public enum DialogActions
    {
        Create,
        Edit,
        Delete
    }

    public class ViewWithDialogModel<T>
    {
        public DialogActions? DialogActions { get; set; }
        public string? DialogTitle { get; set; }
        public T? Item { get; set; }
        public Func<object, HtmlString>? Content { get; set; }

    }
}

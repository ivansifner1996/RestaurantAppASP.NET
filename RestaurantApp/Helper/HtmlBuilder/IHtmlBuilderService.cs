using RestaurantApp.Models;

namespace RestaurantApp.Helper
{
    public interface IHtmlBuilderService
    {
        string BuildProductList(string template, Menu menu);
        string BuildAddProduct(string template, Menu menu, Product product);
        string BuildDeleteProducts(string template, Menu menu);

    }
}

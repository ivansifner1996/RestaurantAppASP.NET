using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using RestaurantApp.Models;

namespace RestaurantApp.Helper
{
    public class HtmlBuilderService : IHtmlBuilderService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public HtmlBuilderService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public string BuildProductList(string template, Menu menu)
        {

            try
            {
                string templateFilePath = GetTemplateFilePath(template);
                string templateContent = ReadFileContent(templateFilePath);

                templateContent = ReplacePlaceholder(templateContent, "[[[MenuNameValue]]]", menu.Name);
                templateContent = ReplacePlaceholder(templateContent, "[[[MenuDescriptionValue]]]", menu.Description);
                templateContent = ReplacePlaceholder(templateContent, "[[[MenuName]]]", $"menu_0__Name");
                templateContent = ReplacePlaceholder(templateContent, "[[[MenuDescription]]]", $"menu_0__Description");
                templateContent = ReplacePlaceholder(templateContent, "[[[MenuNameInput]]]", $"menu[0].Name");
                templateContent = ReplacePlaceholder(templateContent, "[[[MenuDescriptionInput]]]", $"menu[0].Description");

                if (menu.Products.Any())
                {
                    string productTemplate = ReadFileContent(GetTemplateFilePath("ProdList.cshtml"));
                    StringBuilder productHtml = new StringBuilder();
                    int index = 0;

                    productHtml.AppendLine("<div class='product-list'>");
                    foreach(Product product in menu.Products)
                    {
                        string productData = ReplacePlaceholder(productTemplate, "[[[ProdName]]]", $"menu_0__Products_{index}__Name");
                        productData = ReplacePlaceholder(productData, "[[[ProdPrice]]]", $"menu_0__Products_{index}__Price");
                        productData = ReplacePlaceholder(productData, "[[[ProdDescription]]]", $"menu_0__Products_{index}__Description");

                        productData = ReplacePlaceholder(productData, "[[[ProdNameInput]]]", $"menu[0].Products[{index}].Name");
                        productData = ReplacePlaceholder(productData, "[[[ProdPriceInput]]]", $"menu[0].Products[{index}].Price");
                        productData = ReplacePlaceholder(productData, "[[[ProdDescriptionInput]]]", $"menu[0].Products[{index}].Description");

                        productData = ReplacePlaceholder(productData, "[[[ProdNameValue]]]", product.Name);
                        productData = ReplacePlaceholder(productData, "[[[ProdPriceValue]]]", product.Price.ToString());
                        productData = ReplacePlaceholder(productData, "[[[ProdDescriptionValue]]]", product.Description);

                        productHtml.AppendLine(productData);
                        productHtml.AppendLine($"<input type='hidden' name='menu[0].Products[{index}].ProductID' value={product.ProductID} />");

                        index++;
                    }
                    productHtml.AppendLine($"<input type = 'hidden' for= 'menu_0__MenuId' class='form-control' value='{menu.MenuId}' name='menu[0].MenuId'/>");
                    return templateContent + productHtml.AppendLine("</div>").ToString();
                }
                Debug.Write(templateContent);
                return templateContent; 


            }
            catch (Exception ex)
            {
                //    Log.Error("An error occurred while seeding the database  {Error} {StackTrace} {InnerException} {Source}",
                //        ex.Message, ex.StackTrace, ex.InnerException, ex.Source);
                //}
                return string.Empty;
            }

        }

        public string BuildAddProduct(string template, Menu menu, Product product)
		{
			try
			{
                string templateFilePath = GetTemplateFilePath(template);
                string productTemplate = ReadFileContent(templateFilePath);
                StringBuilder productHtml = new StringBuilder();


                productHtml.AppendLine($"<input type = 'hidden' for= 'menu_0__Name' class='form-control' value='{menu.Name}' name='menu[0].Name'/>");
                productHtml.AppendLine($"<input type = 'hidden' for= 'menu_0__MenuId' class='form-control' value='{menu.MenuId}' name='menu[0].MenuId'/>");

                productHtml.AppendLine("<div class='product-list'>");


                productTemplate = ReplacePlaceholder(productTemplate, "[[[ProdName]]]", $"menu_0__Products_0__Name");
                productTemplate = ReplacePlaceholder(productTemplate, "[[[ProdPrice]]]", $"menu_0__Products_0__Price");
                productTemplate = ReplacePlaceholder(productTemplate, "[[[ProdDescription]]]", $"menu_0__Products_0__Description");

                productTemplate = ReplacePlaceholder(productTemplate, "[[[ProdNameInput]]]", $"menu[0].Products[0].Name");
                productTemplate = ReplacePlaceholder(productTemplate, "[[[ProdPriceInput]]]", $"menu[0].Products[0].Price");
                productTemplate = ReplacePlaceholder(productTemplate, "[[[ProdDescriptionInput]]]", $"menu[0].Products[0].Description");

                productTemplate = ReplacePlaceholder(productTemplate, "[[[ProdNameValue]]]", product.Name ?? "");
                productTemplate = ReplacePlaceholder(productTemplate, "[[[ProdPriceValue]]]", product.Price.ToString() ?? "0");
                productTemplate = ReplacePlaceholder(productTemplate, "[[[ProdDescriptionValue]]]", product.Description ?? "");

                return productHtml + productTemplate + "</div>";

            }
            catch (Exception ex)
			{
                Debug.Write(ex.ToString());
                return String.Empty;
            }
        }

        public string BuildDeleteProducts(string template, Menu menu)
        {
            try
            {
                string templateFilePath = GetTemplateFilePath(template);
                string productTemplate = ReadFileContent(templateFilePath);
                StringBuilder productHtml = new StringBuilder();

                productTemplate = ReplacePlaceholder(productTemplate, "[[[MenuName]]]", "selectedProducts");
                productTemplate = ReplacePlaceholder(productTemplate, "[[[MenuValue]]]", menu.Name);


                if (menu.Products.Any())
                {
                    int tableStartIndex = productTemplate.IndexOf("<section>");
                    int tableEndIndex = productTemplate.IndexOf("</section>", tableStartIndex);

                    string tableContent = productTemplate.Substring(tableStartIndex + "<section>".Length, tableEndIndex - (tableStartIndex + "</section>".Length));
                    int cnt = 0;

                    StringBuilder modifiedTableContent = new StringBuilder(tableContent);
                    foreach (Product product in menu.Products)
                    {
                        if (cnt % 3 == 0)
                        {
                            modifiedTableContent.AppendLine("</div><div class='input-wrapper' style='flex-direction:row'>");
                        }

                        modifiedTableContent.AppendLine($"<label><input type='checkbox' name='selectedProducts' value='{product.Name}'/>{product.Name}</label>");
      
                        cnt++;
                    }

                    productTemplate = productTemplate.Substring(0, tableStartIndex + "<section>".Length) +
                                                                     modifiedTableContent.ToString() +
                                                                     productTemplate.Substring(tableEndIndex);
                }


                productHtml.AppendLine($"<input type = 'hidden' for= 'menu_0__Name' class='form-control' value='{menu.Name}' name='menu[0].Name'/>");
                productHtml.AppendLine($"<input type = 'hidden' for= 'menu_0__MenuId' class='form-control' value='{menu.MenuId}' name='menu[0].MenuId'/>");

                return productTemplate + productHtml;

            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return String.Empty;
            }
        }

        private string GetTemplateFilePath(string templateFileName)
        {
            var templateFilePath = _hostingEnvironment.ContentRootPath + $"{Path.DirectorySeparatorChar}Helper{Path.DirectorySeparatorChar}CshtmlTemplates{Path.DirectorySeparatorChar}{templateFileName}";
            return templateFilePath;
        }

        private string ReadFileContent(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        private string ReplacePlaceholder(string template, string placeholder, string replacement)
        {
            return template.Replace(placeholder, replacement ?? "");
        }


    }

}

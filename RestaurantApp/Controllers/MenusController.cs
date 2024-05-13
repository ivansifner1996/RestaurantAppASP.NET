using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Helper;
using RestaurantApp.Models;
using RestaurantApp.Models.ViewModels;

namespace RestaurantApp.Controllers
{

    public class MenusController : Controller
    {
        private readonly RestaurantContext _context;
        private readonly IHtmlBuilderService _htmlBuilderService;

        public MenusController(RestaurantContext context, IHtmlBuilderService htmlBuilderService)
        {
            _context = context;
            _htmlBuilderService = htmlBuilderService;
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {

            var viewModel = new MenuViewModel<List<Menu>>
            {
                menu = await _context.Menus
                 .Include(p => p.Products)
                 .ToListAsync(),
            };

            ViewBag.Open = "none";
            return View(viewModel);
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.Restaurant)
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menus/Create
        public async Task<IActionResult> Create(int id, string? error)
        {
            var menu = await _context.Menus
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.MenuId == id);

            var menuObj = TempData["ProductData"] as string;
            Product product = new Product();

            if (menuObj != null)
            {
                product = JsonSerializer.Deserialize<Product>(menuObj);
                menu.Products.Add(product);
            }

            var addProducts = _htmlBuilderService.BuildAddProduct("ProdList.cshtml", menu, product);


            var viewModel = new MenuViewModel<List<Menu>>
            {
                DialogActions = DialogActions.Create,
                DialogTitle = "Add Product",
                menu = new List<Menu> { menu },
                Content = (content) =>
                {
                    return new HtmlString(addProducts);
                }
            };

            if (!string.IsNullOrEmpty(error))
            {
               ModelState.AddModelError(String.Empty, error);
            }

            ViewBag.Open = "block";
            return View("~/Views/Menus/Index.cshtml", viewModel);
        }

        // POST: Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuViewModel<List<Menu>> viewModel)
        {
            string errorMessages = "";
            var menu = viewModel.menu[0];

            Product product = new Product
            {
                Name = menu.Products[0]?.Name,
                Description = menu.Products[0]?.Description,
                Price = (decimal)(menu.Products[0]?.Price),
                MenuId = menu.MenuId
            };

            if (ModelState.IsValid)
            { 

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var modelStateVal in ViewData.ModelState.Values)
                {
                    foreach (var err in modelStateVal.Errors)
                    {
                        var errorMessage = err.ErrorMessage;
                        var exception = err.Exception;
                        errorMessages = string.Join(";", errorMessage);
                    }
                }
                var menuJson = JsonSerializer.Serialize(product);
                TempData["ProductData"] = menuJson;
            }

            return RedirectToAction(nameof(Create), new {error = errorMessages});

        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.MenuId == id);

            if (menu == null)
            {
                return NotFound();
            }
            var editData = _htmlBuilderService.BuildProductList("EditProducts.cshtml", menu);
            var viewModel = new MenuViewModel<List<Menu>>
            {
                DialogActions = DialogActions.Edit,
                DialogTitle = "Edit Menu",
                menu = new List<Menu> { menu },
                Content = (content) =>
                {
                    return new HtmlString(editData);
                }
            };

            ViewBag.Open = "block";
            return View("~/Views/Menus/Index.cshtml", viewModel);

        }
        // POST: Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuViewModel<List<Menu>> viewModel)
        {
            Menu menu = viewModel.menu[0];
        
            if (ModelState.IsValid)
            {
                try
                {
                    var products = new List<Product>();

                    if (menu.Products != null)
                    {
                        foreach (var product in menu.Products)
                        {
                            product.MenuId = menu.MenuId;
                            products.Add(product);
                        }

                        _context.Products.UpdateRange(products);

                    }
                    var menuToUpdate = await _context.Menus.FindAsync(menu.MenuId);

                    if (menuToUpdate == null)
                    {
                        return NotFound(); // Menu not found
                    }

                    menuToUpdate.Name = menu.Name;
                    menuToUpdate.Description = menu.Description;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!MenuExists(viewModel.menu[0].MenuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Open = "block";
            return View("~/Views/Menus/Index.cshtml", viewModel);
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id, string? error)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.MenuId == id);

            if (menu == null)
            {
                return NotFound();
            }

            var deleteData = _htmlBuilderService.BuildDeleteProducts("DeleteProducts.cshtml", menu);
            var viewModel = new MenuViewModel<List<Menu>>
            {
                DialogActions = DialogActions.Delete,
                DialogTitle = "Delete Products",
                menu = new List<Menu> { menu },
                Content = (content) =>
                {
                    return new HtmlString(deleteData);
                }
            };

            if (error != null)
            {
                ModelState.AddModelError(String.Empty, error);
            }

            ViewBag.Open = "block";
            return View("~/Views/Menus/Index.cshtml", viewModel);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string[] selectedProducts)
        {
            var menu = await _context.Menus.FindAsync(id);

            if (menu == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var ordersWithSelectedProducts = await _context.Orders
                .Include(o => o.Product)
                    .ThenInclude(p => p.Menu)
                .Where(o => selectedProducts.Contains(o.Product.Name) && o.Product.Menu.MenuId == menu.MenuId)
                .ToListAsync();

             if (ordersWithSelectedProducts.Any())
             {
                  var orderIds = string.Join(", ", ordersWithSelectedProducts.Select(o => o.OrderId));
                  string errorMessage = $"Cannot delete products due to active orders: Orders {orderIds} are using selected products";
                  return RedirectToAction(nameof(Delete), new {id = id, error = errorMessage});
             }

            IActionResult deletionResult = await DeleteSelectedItems(selectedProducts, menu);

            return deletionResult;
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.MenuId == id);
        }

        private async Task<IActionResult> DeleteSelectedItems(string[] selectedProducts, Menu menu)
        {

            if (selectedProducts == null || selectedProducts.Length == 0)
            {
                string errorMessage = "No products or menus are selected for deletion";
                return RedirectToAction(nameof(Delete), new { id = menu.MenuId, error = errorMessage });
            }

            var allMenuProducts = new HashSet<string>
                (await _context.Menus.Where(m => m.MenuId == menu.MenuId)
                .Select(m => m.Products.Select(p => p.Name))
                .FirstOrDefaultAsync());


            bool anyMenu = selectedProducts.Contains(menu.Name);

            bool deleteAll = allMenuProducts.SetEquals(selectedProducts);

            foreach (var product in selectedProducts)
            {

                if (!deleteAll && anyMenu)
                {
                    string errormessage = "Operation can't proceed because multiple products are linked to your selected menu item";
                    return RedirectToAction(nameof(Delete), new {id= menu.MenuId, error = errormessage});

                }
                else
                {
                    Product productToRemove = await _context.Products.FirstOrDefaultAsync(p => p.Name == product);
                    _context.Products.Remove(productToRemove);

                    if (anyMenu)
                    {
                        _context.Menus.Remove(menu);
                    }
                }
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok("deletion successful.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"error in db: {ex.Message}");


            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Models.ViewModels;

namespace RestaurantApp.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly RestaurantContext _context;

        public RestaurantsController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index(int? id, int? menuId)
        {
            var viewModel = new RestaurantCollectedData();
            viewModel.Restaurants = await _context.Restaurants
                .Include(r => r.Menus)
                    .ThenInclude(r => r.Products)
                .AsNoTracking()
                .ToListAsync();

            if(id != null)
            {
                Restaurant restaurant = viewModel.Restaurants.Where(
                    r => r.ID == id.Value).Single();
                viewModel.Menus = restaurant.Menus;
                ViewData["RestaurantId"] = id.Value;
            }

            if(menuId != null)
            {
                viewModel.Products = viewModel.Menus.Where(
                    m => m.MenuId == menuId).Single().Products;
                ViewData["MenuId"] = menuId.Value;
            }
            return View(viewModel);
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(m => m.ID == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            ViewData["Visibility"] = "none";
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FoundationDate,Country, RestaurantName, Address, Menus")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);

                if (restaurant.Menus != null && restaurant.Menus.Any())
                {
                    var duplicates = CheckMenusDuplicates(restaurant.Menus);

                    if (duplicates.Any())
                    {
                       string errorMessage = "Menus names: " + string.Join(", ", duplicates) + " are not unique";

                        ModelState.AddModelError(string.Empty, errorMessage);
                        return View(restaurant);
                    }

                    foreach (var menu in restaurant.Menus)
                    {
                        _context.Add(menu);
                    }
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["Visibility"] = "block";
            return View(restaurant);

        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FoundationDate,Country,RestaurantName,Address")] Restaurant restaurant)
        {
            if (id != restaurant.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.ID))
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
            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurants.Any(e => e.ID == id);
        }

        private List<string> CheckMenusDuplicates(List<Menu> menus)
        {
            var duplicates = menus.Select(m => m.Name.ToLower())
                    .GroupBy(m => m)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

            return duplicates;
        }
    }
}

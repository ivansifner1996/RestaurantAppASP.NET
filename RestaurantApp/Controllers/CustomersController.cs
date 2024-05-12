using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.Models;
using RestaurantApp.Utilities;

namespace ContosoUniversity.Controllers
{
    public class CustomersController : Controller
    {
        private readonly RestaurantContext _context;

        public CustomersController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string sortType, string searchTerm, string searchFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortType;
            ViewData["CustomerSort"] = String.IsNullOrEmpty(sortType) ? "customer-desc" : "";
            ViewData["LastNameSort"] = String.IsNullOrEmpty(sortType) ? "lastName-desc" : "";
            ViewData["FirstNameSort"] = String.IsNullOrEmpty(sortType) ? "firstName-desc" : "";
            ViewData["IBANSort"] = String.IsNullOrEmpty(sortType) ? "IBAN-desc" : "";

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = searchFilter;
            }

            ViewData["SearchFilter"] = searchTerm;

            var customers = from c in _context.Customers
                           select c;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                customers = customers.Where(s => s.CustomerName.Contains(searchTerm));
            }

            customers = sortType switch
            {
                "customer-desc" => customers.OrderByDescending(s => s.CustomerName),
                "lastName-desc" => customers.OrderByDescending(s => s.OwnerLastName),
                "firstName-desc" => customers.OrderByDescending(s => s.OwnerFirstName),
                "IBAN-desc" => customers.OrderByDescending(s => s.IBAN),
                _ => customers.OrderBy(s => s.CustomerName),
            };

            Pagination<Customer> customerPagination = new Pagination<Customer>(customers.AsNoTracking(), pageNumber ?? 1);
            return View(customerPagination);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(s => s.Orders)
                    .ThenInclude(e => e.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OwnerFirstName,OwnerLastName,CustomerName, IBAN")] Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
            "Try again, and if the problem persists " +
            "see your system administrator.");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Customers.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerToUpdate = await _context.Customers.FirstOrDefaultAsync(x => x.ID == id);
            if (customerToUpdate == null)
            {
                return NotFound();
            }
            var includeProperties = new string[] { "OwnerLastName", "OwnerFirstName", "CustomerName", "IBAN" };


            if (await TryUpdateModelAsync<Customer>(customerToUpdate, "", MapCustomerProperties(includeProperties)))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
               "Try again, and if the problem persists, " +
               "see your system administrator.");
                    throw;
                }
            }

            return View(customerToUpdate);

        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }


        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.ID == id);
        }

        private Expression<Func<Customer, object>>[] MapCustomerProperties(IEnumerable<string> properties)
        {
            return properties.Select(propertyName =>
            {
                var parameter = Expression.Parameter(typeof(Customer));
                var property = Expression.Property(parameter, propertyName);
                return Expression.Lambda<Func<Customer, object>>(property, parameter);
            }).ToArray();
        }
    }
}

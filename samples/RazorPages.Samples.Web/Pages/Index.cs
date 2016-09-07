using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RazorPages.Samples.Web.Data;

namespace RazorPages.Samples.Web.Pages
{
    public class Index : Page
    {
        private readonly ILogger<Index> _logger;

        public Index(AppDbContext db, ILogger<Index> logger)
        {
            Db = db;
            _logger = logger;
        }

        public AppDbContext Db { get; }

        public IEnumerable<Customer> ExistingCustomers { get; set; }

        public Customer Customer { get; set; } = new Customer();

        [TempData]
        public string SuccessMessage { get; set; }

        public bool ShowSuccessMessage => !string.IsNullOrEmpty(SuccessMessage);

        [TempData]
        public string ErrorMessage { get; set; }

        public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public void OnGet()
        {
            ExistingCustomers = Db.Customers.AsNoTracking().ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await TryUpdateModelAsync(Customer, nameof(Customer));

            if (!ModelState.IsValid)
            {
                // Model errors, populate customer list and show errors
                ExistingCustomers = Db.Customers.AsNoTracking().ToList();
                ErrorMessage = "Please correct the errors and try again";
                return View();
            }

            Db.Add(Customer);
            await Db.SaveChangesAsync();

            SuccessMessage = $"Customer {Customer.Id} successfully created!";
            return Redirect("/");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            _logger.LogInformation("Performing delete of customer with ID {customerId}", id);

            Db.Remove(new Customer { Id = id });
            await Db.SaveChangesAsync();

            SuccessMessage = $"Customer {id} deleted successfully!";
            return Redirect("/");
        }
    }
}

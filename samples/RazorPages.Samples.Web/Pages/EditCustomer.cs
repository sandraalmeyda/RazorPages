using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Samples.Web.Data;

namespace RazorPages.Samples.Web.Pages
{
    public class EditCustomer : Page
    {
        public EditCustomer(AppDbContext db)
        {
            Db = db;
        }

        public AppDbContext Db { get; }

        public Customer CurrentCustomer { get; set; }

        public string SuccessMessage { get; set; }

        public bool ShowSuccessMessage => !string.IsNullOrEmpty(SuccessMessage);

        [TempData]
        public string ErrorMessage { get; set; }

        public bool ShowErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public IActionResult OnGet(int id)
        {
            CurrentCustomer = GetCustomer(id);
            return View(CurrentCustomer);
        }

        public IActionResult OnPost(Customer customer)
        {
            if (ModelState.IsValid)
            {
                UpdateCustomer(customer);
                SuccessMessage = $"Customer {customer.Id} successfully edited!";
                return Redirect("/");
            }

            // Model errors, populate customer list and show errors
            CurrentCustomer = GetCustomer(customer.Id);
            ErrorMessage = "Please correct the errors and try again";
            return View();
        }

        public Customer GetCustomer(int id)
        {
            var customer = Db.Customers.Where(c => c.Id == id).SingleOrDefault();

            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            var currentCustomer = Db.Customers.Single(c => c.Id == customer.Id);
            
            currentCustomer.Name = customer.Name;
            Db.SaveChanges();
        }
    }
}

using System.Linq;
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

        public Data.Customer GetCustomer(int id)
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

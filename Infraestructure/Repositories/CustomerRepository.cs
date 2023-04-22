using Infraestructure.Database;
using Infraestructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> ListCustomers();
        Task<Customer> GetCustomerById(int customerId);
        Task<bool> SaveCustomer(Customer customer);
    }
    public class CustomerRepository: ICustomerRepository
    {
        protected Context _ctx;

        public CustomerRepository(Context ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<Customer>> ListCustomers()
        {
            return _ctx.Customer.ToList();
        }
        public async Task<Customer> GetCustomerById(int customerId)
        {
            return _ctx.Customer.Where(x => x.CustomerId == customerId).FirstOrDefault();
        }
        public async Task<bool> SaveCustomer(Customer customer)
        {
            _ctx.Customer.Add(customer);
            return _ctx.SaveChanges() > 0;
        }

    }
}

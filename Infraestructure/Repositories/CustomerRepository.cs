using Infraestructure.Database;
using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
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
        Task<bool> UpdateCustomer(Customer customer);
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
            var customers = _ctx.Customer.Include("IdentificationType").ToList();
            return customers;
        }
        public async Task<Customer> GetCustomerById(int customerId)
        {
            return _ctx.Customer.Include("IdentificationType").Where(x => x.Id == customerId).FirstOrDefault();
        }
        public async Task<bool> UpdateCustomer(Customer customer)
        {
            _ctx.Customer.Update(customer);
            return _ctx.SaveChanges() > 0;
        }
        public async Task<bool> SaveCustomer(Customer customer)
        {
            _ctx.Customer.Add(customer);
            return _ctx.SaveChanges() > 0;
        }

    }
}

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
    public interface ISaleRepository
    {
        Task<List<Sale>> ListSales();
        Task<Sale> GetCustomerById(int saleId);
        Task<bool> UpdateSale(Sale sale);
        Task<bool> SaveSale(Sale sale);
    }
    public class SaleRepository: ISaleRepository
    {
        protected Context _ctx;

        public SaleRepository(Context ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<Sale>> ListSales()
        {
            var sales = _ctx.Sale.Include("Customer").Include("SaleStatus").Include("Customer.IdentificationType").ToList();
            return sales;
        }

        public async Task<Sale> GetCustomerById(int saleId)
        {
            return _ctx.Sale.Include("Customer").Include("SaleStatus").Include("Customer.IdentificationType").Where(x => x.Id == saleId).FirstOrDefault();
        }
        public async Task<bool> UpdateSale(Sale sale)
        {
            _ctx.Sale.Update(sale);
            return _ctx.SaveChanges() > 0;
        }
        public async Task<bool> SaveSale(Sale sale)
        {
            _ctx.Sale.Add(sale);
            return _ctx.SaveChanges()>0;
        }
    }
}

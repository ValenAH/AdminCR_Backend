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
        Task<List<Sale>> ListCreditSales();
        Task<Sale> GetSaleById(int saleId);
        Task<bool> UpdateSale(Sale sale);
        Task<int> SaveSale(Sale sale);
        Task<List<Sale>> GetSalesByCustomerId(int customerId);
        Task<int> GetConsecutive();
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
            var sales = _ctx.Sale.Include("Customer").Include("SaleStatus").Include("Customer.IdentificationType").Include("SaleDetails").ToList();
            return sales;
        }
        public async Task<List<Sale>> ListCreditSales()
        {
            var creditSales = _ctx.Sale.Include("Customer").Include("SaleStatus").Include("Customer.IdentificationType").Include("SaleDetails").Where(x => x.isCredit == true).ToList();
            return creditSales;
        }

        public async Task<Sale> GetSaleById(int saleId)
        {
            return _ctx.Sale.Include("Customer").Include("SaleStatus").Include("Customer.IdentificationType").Include("SaleDetails").Include("SaleDetails.Product").Where(x => x.Id == saleId).FirstOrDefault();
        }
        public async Task<bool> UpdateSale(Sale sale)
        {
            _ctx.Sale.Update(sale);
            return _ctx.SaveChanges() > 0;
        }
        public async Task<int> SaveSale(Sale sale)
        {
            _ctx.Sale.Add(sale);
            _ctx.SaveChanges();
            return sale.Id;
        }
        public async Task<List<Sale>> GetSalesByCustomerId(int customerId)
        {
            var sales = _ctx.Sale.Include("SaleStatus").Where(x => x.CustomerId == customerId).ToList();
            return sales;
        }

        public async Task<int> GetConsecutive()
        {
            var count = _ctx.Sale.Count();
            return count;
        }
    }
}

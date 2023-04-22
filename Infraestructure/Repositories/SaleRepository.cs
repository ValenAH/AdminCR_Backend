using Infraestructure.Database;
using Infraestructure.Database.Entities;
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
            return _ctx.Sale.ToList();
        }

        public async Task<Sale> GetCustomerById(int saleId)
        {
            return _ctx.Sale.Where(x => x.SaleId == saleId).FirstOrDefault();
        }
        public async Task<bool> SaveSale(Sale sale)
        {
            _ctx.Sale.Add(sale);
            return _ctx.SaveChanges()>0;
        }
    }
}

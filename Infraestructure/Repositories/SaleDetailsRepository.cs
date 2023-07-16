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
    public interface ISaleDetailsRepository
    {
        Task<List<SaleDetails>> ListSaleDetails(int saleId);
    }
    public class SaleDetailsRepository : ISaleDetailsRepository
    {
        protected Context _ctx;
        public SaleDetailsRepository(Context ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<SaleDetails>> ListSaleDetails(int saleId)
        {
            return _ctx.SaleDetails.Include("Product").Include("Product.Category").Where(x => x.SaleId == saleId).ToList();
        }
    }
}

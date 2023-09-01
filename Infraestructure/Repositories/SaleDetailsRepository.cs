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
        Task<bool> SaveSaleDetails(List<SaleDetails> list);
        Task<int> DeleteSaleDetail(int saleDetailId);
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

        public async Task<bool> SaveSaleDetails (List<SaleDetails> list)
        {
            _ctx.AddRange(list);
            return _ctx.SaveChanges() > 0;
        }

        public async Task<int> DeleteSaleDetail(int saleDetailId)
        {
            var saleDetail = _ctx.SaleDetails.Where(x => x.Id == saleDetailId).FirstOrDefault();
            _ctx.SaleDetails.Remove(saleDetail);
            return saleDetail.SaleId;
        }
    }
}

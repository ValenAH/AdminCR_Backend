using Infraestructure.Database;
using Infraestructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public interface IQuotaRepository
    {
        Task<List<Quota>> ListQuotas();
        Task<Quota> GetQuotaBySaleId(int id);
        Task<bool> UpdateQuota(Quota quota);
        Task<bool> SaveQuota(Quota quota);
    }
    public class QuotaRepository : IQuotaRepository
    {
        protected Context _ctx;
        public QuotaRepository(Context ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<Quota>> ListQuotas()
        {
            var quotas = _ctx.Quota.ToList();
            return quotas;
        }
        public async Task<Quota> GetQuotaBySaleId(int id)
        {
            var quota = _ctx.Quota.Where(x=>x.SaleId == id).FirstOrDefault();
            return quota;
        }
        public async Task<bool> UpdateQuota(Quota quota)
        {
            _ctx.Quota.Update(quota);
            return _ctx.SaveChanges()>0;
        }
        public async Task<bool> SaveQuota(Quota quota)
        {
            _ctx.Quota.Add(quota);
            return _ctx.SaveChanges()>0;
        }
    }
}

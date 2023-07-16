using Infraestructure.Database;
using Infraestructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public interface IIdentificationTypeRepository
    {
        Task<List<IdentificationType>> ListIdentificationType();
    }
    public class IdentificationTypeRepository: IIdentificationTypeRepository
    {
        protected Context _ctx;
        public IdentificationTypeRepository(Context ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<IdentificationType>> ListIdentificationType()
        {
            var identificationType = _ctx.IdentificationType.ToList();
            return identificationType;
        }
    }
}

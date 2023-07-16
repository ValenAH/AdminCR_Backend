using Infraestructure.Database;
using Infraestructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories
{
    public interface IPaymentMethodRepository
    {
        Task<List<PaymentMethod>> ListPaymentMethods();
    }
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private Context _ctx;

        public PaymentMethodRepository(Context ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<PaymentMethod>> ListPaymentMethods()
        {
            var paymentMethods = _ctx.PaymentMethod.ToList();
            return paymentMethods;
        }
    }
}

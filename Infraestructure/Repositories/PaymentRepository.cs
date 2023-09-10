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
    public interface IPaymentRepository
    {
        Task<List<Payment>> ListPaymentBySale(int saleId);
        Task<bool> SavePayment(List<Payment> payment);
    }
    public class PaymentRepository : IPaymentRepository
    {
        protected Context _ctx;

        public PaymentRepository(Context ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Payment>> ListPaymentBySale(int saleId)
        {
            var payments = _ctx.Payment.Include("PaymentMethod").Where(x => x.SaleId == saleId).ToList();
            return payments;
        }

        public async Task<bool> SavePayment(List<Payment> payment)
        {
            _ctx.Payment.AddRange(payment);
            return _ctx.SaveChanges() > 0;
        }
    }
}

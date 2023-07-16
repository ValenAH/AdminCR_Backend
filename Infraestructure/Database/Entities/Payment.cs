using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public int SaleId { get; set; }
        public decimal Amount { get; set; }
    }
}

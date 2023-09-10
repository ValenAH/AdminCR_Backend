using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethodDTO? PaymentMethod { get; set; }
        public int SaleId { get; set; }
        public decimal Amount { get; set; }
        public string Date  = DateTime.Today.ToString("d");
    }
}

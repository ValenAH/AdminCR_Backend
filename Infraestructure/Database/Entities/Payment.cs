using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    [Table("payment")]
    public class Payment
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("payment_method_id")]
        public int PaymentMethodId { get; set; }

        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod? PaymentMethod { get; set; }

        [Column("sale_id")]
        public int SaleId { get; set; }

        [ForeignKey(nameof(SaleId))]
        public Sale? Sale { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    [Table("sale")]
    public class Sale
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("consecutive")]
        public string Consecutive { get; set; }

        [Column("sale_date")]
        public DateTime SaleDate { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        [Column("delivery_date")]
        public DateTime DeliveryDate { get; set; }

        [Column("sale_status_id")]
        public int SaleStatusId { get; set; }

        [ForeignKey(nameof(SaleStatusId))]
        public SaleStatus? SaleStatus { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("is_credit")]
        public bool isCredit { get; set; }

        public List<SaleDetails>? SaleDetails { get; set; }
    }
}

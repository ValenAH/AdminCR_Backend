using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    [Table("sale_details")]
    public class SaleDetails
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("tax")]
        public decimal? Tax { get; set; }

        [Column("sale_id")]
        public int SaleId { get; set; }
    }
}

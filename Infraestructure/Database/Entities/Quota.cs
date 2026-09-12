using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    [Table("quota")]
    public class Quota
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("is_payed")]
        public bool IsPayed { get; set; }

        [Column("sale_id")]
        public int SaleId { get; set; }

        [ForeignKey(nameof(SaleId))]
        public Sale? Sale { get; set; }
    }
}

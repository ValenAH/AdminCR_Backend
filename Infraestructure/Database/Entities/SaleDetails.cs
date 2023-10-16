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
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal? Tax { get; set; }
        public int SaleId { get; set; }
    }
}

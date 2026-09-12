using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    [Table("sale_status")]
    public class SaleStatus
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    [Table("payment_method")]
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
    }
}

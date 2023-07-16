using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public string Consecutive { get; set; } 
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int SaleStatusId { get; set; }
        public SaleStatus? SaleStatus { get; set; }

    }
}

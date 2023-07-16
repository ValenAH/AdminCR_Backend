using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
    }
}

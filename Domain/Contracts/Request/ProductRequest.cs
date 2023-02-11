using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Request
{
    internal class ProductRequest
    {
        public int IdProduct { get; set; }
        public string? ProductName { get; set; }
        public int UnitCost { get; set; }
        public int Price { get; set; }
        public string? IdCategory { get; set; }
    }
}

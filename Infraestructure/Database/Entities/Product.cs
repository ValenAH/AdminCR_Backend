using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Database.Entities
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }
        public string? ProductName { get; set; }
        public int UnitCost { get; set; }
        public int Price { get; set; }

    }
}

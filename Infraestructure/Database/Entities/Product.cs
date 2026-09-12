using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Database.Entities
{
    [Table("product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int UnitCost { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public bool Enable { get; set; }

    }
}

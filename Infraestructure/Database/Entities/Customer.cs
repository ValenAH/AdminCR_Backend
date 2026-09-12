using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    [Table("customer")]
    public class Customer
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("telephone")]
        public string Telephone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("identification_type_id")]
        public int IdentificationTypeId { get; set; }

        [Column("identification_number")]
        public string IdentificationNumber { get; set; }

        [ForeignKey(nameof(IdentificationTypeId))]
        public virtual IdentificationType? IdentificationType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Database.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("username")]
        public string? UserName { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("role_id")]
        public int IdRole { get; set; }

        [Column("enable")]
        public bool Enable { get; set; }
    }
}

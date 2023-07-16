using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int IdentificationTypeId { get; set; }
        public virtual IdentificationTypeDTO? IdentificationType { get; set; }
        public string? IdentificationNumber { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO
{
    public class SaleDTO
    {
        public int Id { get; set; }
        public string? Consecutive { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public CustomerDTO? Customer { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int SaleStatusId { get; set; }
        public SaleStatusDTO? SaleStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleDetailsDTO>? SaleDetails { get; set; }
    }
}


namespace Domain.Contracts.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int UnitCost { get; set; }
        public int CategoryId { get; set; }
        public bool Enable { get; set; }
    }
}

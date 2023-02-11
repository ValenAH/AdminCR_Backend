
namespace Domain.Contracts.DTO
{
    public class ProductDTO
    {
        public int IdProduct { get; set; }
        public string? ProductName { get; set; }
        public int UnitCost { get; set; }
        public int Price { get; set; }
        public string? IdCategory { get; set; }
    }
}

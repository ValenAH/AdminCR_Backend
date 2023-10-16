
namespace Domain.Contracts.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int UnitCost { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryDTO? Category { get; set; }
        public bool Enable { get; set; }
        public int ProductTypeId { get; set; }
    }
}

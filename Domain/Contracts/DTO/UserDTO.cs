

namespace Domain.Contracts.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int IdRole { get; set; }
        public bool Enable { get; set; }
    }  
}

namespace TinyCRM.Application.Models.Account
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Address { get; set; }
        public decimal TotalSale { get; set; }
    }
}
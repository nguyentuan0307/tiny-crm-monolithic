namespace TinyCRM.API.Models.Account
{
    public class AccountCreateDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
    }
}

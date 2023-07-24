namespace TinyCRM.API.Models.Contact
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Guid AccountId { get; set; }
    }
}
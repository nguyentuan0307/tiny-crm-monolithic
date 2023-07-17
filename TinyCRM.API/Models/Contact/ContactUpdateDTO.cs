namespace TinyCRM.API.Models.Contact
{
    public class ContactUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid AccountId { get; set; }
    }
}

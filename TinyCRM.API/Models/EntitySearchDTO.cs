namespace TinyCRM.API.Models
{
    public abstract class EntitySearchDTO
    {
        public string? Filter { get; set; }

        public int SkipCount { get; set; } = 0;

        public int MaxResultCount { get; set; } = 20;

        public string? Sorting { get; set; }
    }
}

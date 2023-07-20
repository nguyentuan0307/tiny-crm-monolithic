namespace TinyCRM.API.Models.Deal
{
    public class DealStatisticDTO
    {
        public int OpenDeals { get; set; }
        public int WonDeals { get; set; }
        public int LostDeals { get; set; }
        public decimal AvgRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
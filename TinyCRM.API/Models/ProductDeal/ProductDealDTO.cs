﻿namespace TinyCRM.API.Models.ProductDeal
{
    public class ProductDealDTO
    {
        public Guid Id { get; set; }
        public Guid DealId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

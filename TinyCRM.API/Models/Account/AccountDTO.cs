﻿namespace TinyCRM.API.Models.Account
{
    public class AccountDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public decimal TotalSale { get; set; }
    }
}
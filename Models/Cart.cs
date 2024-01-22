using System;
using System.Collections.Generic;

#nullable disable

namespace BackendFoodOrder.Models
{
    public partial class Cart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string TotalAmount { get; set; }
        public string DTAdded { get; set; }
    }
}

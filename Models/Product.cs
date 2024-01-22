using System;
using System.Collections.Generic;

#nullable disable

namespace BackendFoodOrder.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Price { get; set; }
        public string Stock { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
    }
}

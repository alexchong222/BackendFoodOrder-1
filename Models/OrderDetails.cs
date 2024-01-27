namespace BackendFoodOrder.Models
{
    public partial class OrderDetails
    {
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string TotalAmount { get; set; }
        public string DTAdded { get; set; }
    }
}

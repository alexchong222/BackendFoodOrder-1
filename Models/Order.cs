namespace BackendFoodOrder.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryStatus { get; set; }
        public string Ratings { get; set; }
        public string DTAdded { get; set; }
    }
}

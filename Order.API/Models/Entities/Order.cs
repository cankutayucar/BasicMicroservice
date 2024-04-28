using Order.API.Models.Enums;

namespace Order.API.Models.Entities
{
    public class Order
    {
        public string OrderId { get; set; }
        public string BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }


        public ICollection<OrderItem> OrderItems { get; set; }
    }
}

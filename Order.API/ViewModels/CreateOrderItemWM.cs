namespace Order.API.ViewModels
{
    public class CreateOrderItemWM
    {
        public int Count { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
    }
}
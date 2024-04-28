namespace Order.API.ViewModels
{
    public class CreateOrderWM
    {
        public string BuyerId { get; set; }
        public List<CreateOrderItemWM> CreateOrderItemWMs { get; set; }
    }
}

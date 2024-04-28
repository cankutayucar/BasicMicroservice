using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer(IPublishEndpoint _publishEndpoint) : IConsumer<StockReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {


            // ödeme işlemleri

            if (true)
            {
                // ödeme başarılı
                await _publishEndpoint.Publish(new PaymentCompletedEvent
                {
                    OrderId = context.Message.OrderId
                });
            }
            else
            {
                // ödeme sıkıntılı
                await _publishEndpoint.Publish(new PaymentFailedEvent
                {
                    OrderId = context.Message.OrderId,
                    Reason = "..."
                });
            }


        }
    }
}

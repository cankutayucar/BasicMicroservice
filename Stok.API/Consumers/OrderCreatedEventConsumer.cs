using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Stok.API.Models.Entities;
using Stok.API.Services;

namespace Stok.API.Consumers
{
    public class OrderCreatedEventConsumer(MongoDbService _mongoDbService, ISendEndpointProvider _sendEndpointProvider, IPublishEndpoint _publishEndpoint) : IConsumer<OrderCreatedEvent>
    {
        IMongoCollection<Stock> stocks => _mongoDbService.GetCollection<Stock>();

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {

            List<bool> stockResult = new();


            foreach (var orderItem in context.Message.OrderItems)
            {
                stockResult.Add((await stocks.FindAsync(x => x.ProductId == orderItem.ProductId && x.Count >= orderItem.Count)).Any());
            }


            if (stockResult.TrueForAll(x => x.Equals(true)))
            {
                // gerekli sipariş işlemleri

                foreach (var orderItem in context.Message.OrderItems)
                {
                    //Stock stock = await stocks.FindOneAndUpdateAsync(x => x.ProductId == orderItem.ProductId, Builders<Stock>.Update.Inc(x => x.Count, -orderItem.Count));


                    var stock = await (await stocks.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();
                    stock.Count -= orderItem.Count;
                    await stocks.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock);
                }

                // payment

                StockReservedEvent stockReservedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    BuyerId = context.Message.BuyerId,
                    TotalPrice = context.Message.TotalPrice,
                };

                var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMqSettings.Payment_Stock_Reserved_Event_Queue}"));
                await endpoint.Send(stockReservedEvent);
            }
            else
            {
                // sipariş işlemi başarısız
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    BuyerId = context.Message.BuyerId,
                    Message = "...",
                };

                await _publishEndpoint.Publish(stockNotReservedEvent);
            }

        }
    }
}

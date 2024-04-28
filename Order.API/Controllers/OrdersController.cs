using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.Models.Entities;
using Order.API.ViewModels;
using Shared.Events;
using Shared.Messages;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(OrderAPIDbContext _orderAPIDbContext, IPublishEndpoint _publishEndpoint) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderWM createOrderWM)
        {
            try
            {


                Order.API.Models.Entities.Order order = new()
                {
                    OrderId = Guid.NewGuid().ToString(),
                    BuyerId = createOrderWM.BuyerId,
                    CreatedDate = DateTime.UtcNow,
                    OrderStatus = Order.API.Models.Enums.OrderStatus.Suspend,
                };
                order.OrderItems = createOrderWM.CreateOrderItemWMs.Select(x => new OrderItem
                {
                    Count = x.Count,
                    OrderId = order.OrderId,
                    ProductId = x.ProductId,
                    Price = x.Price,
                    Id = Guid.NewGuid().ToString()
                }).ToList();
                order.TotalPrice = order.OrderItems.Sum(x => x.Price * x.Count);

                await _orderAPIDbContext.Orders.AddAsync(order);
                await _orderAPIDbContext.SaveChangesAsync();

                OrderCreatedEvent orderCreatedEvent = new()
                {
                    OrderId = order.OrderId,
                    BuyerId = order.BuyerId,
                    OrderItems = order.OrderItems.Select(x => new OrderItemMessage
                    {
                        Count = x.Count,
                        ProductId = x.ProductId
                    }).ToList(),
                    TotalPrice = order.TotalPrice
                };
                await _publishEndpoint.Publish(orderCreatedEvent);

                return Ok();
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}

using Shared.Events.Common;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class OrderCreatedEvent : IEvent
    {
        public string OrderId { get; set; }
        public string BuyerId { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; }

        public decimal TotalPrice { get; set; }
    }
}

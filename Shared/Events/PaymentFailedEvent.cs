using Shared.Events.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class PaymentFailedEvent : IEvent
    {
        public string OrderId { get; set; }
        public string Reason { get; set; }
    }
}

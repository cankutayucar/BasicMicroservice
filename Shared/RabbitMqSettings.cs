using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class RabbitMqSettings
    {
        public const string Stock_Order_Created_Event_Queue = "stock-order-created-event-queue";
        public const string Payment_Stock_Reserved_Event_Queue = "payment-stock-reserved-event-queue";
        public const string Order_Payment_Completed_Event_Queue = "order-payment-completed-event-queue";
        public const string Order_Stock_Not_Reserved_Event_Queue = "order-stock-not-reserved-event-queue";
        public const string Order_Payment_Failed_Event_Queue = "order-payment-failed-event-queue";
    }
}

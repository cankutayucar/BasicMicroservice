﻿using Shared.Events.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StockNotReservedEvent : IEvent
    {
        public string BuyerId { get; set; }
        public string OrderId { get; set; }
        public string Message { get; set; }
    }
}

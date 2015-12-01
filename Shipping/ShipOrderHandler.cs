using System;

namespace Shipping
{
    using Messages;
    using NServiceBus;
    public class ShipOrderHandler: IHandleMessages<ShipOrder>
    {
        public void Handle(ShipOrder message)
        {
            Console.Out.WriteLine($"Order {message.OrderId} is shipping");
        }
    }
}

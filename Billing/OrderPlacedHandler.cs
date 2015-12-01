namespace Billing
{
    using System.Threading;
    using Contracts;
    using NServiceBus;
    using Sales.Contracts;

    public class OrderPlacedHandler: IHandleMessages<OrderPlaced>
    {
        public IBus Bus { get; set; }

        public void Handle(OrderPlaced message)
        {
            Thread.Sleep(5000);
            Bus.Publish<OrderBilled>(e => e.OrderId = message.OrderId);
        }
    }
}

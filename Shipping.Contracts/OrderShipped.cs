namespace Shipping.Contracts
{
    using NServiceBus;
    public class OrderShipped: IEvent
    {
        public string OrderId { get; set; }
    }
}

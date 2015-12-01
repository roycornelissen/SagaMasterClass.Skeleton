namespace Billing.Contracts
{
    using NServiceBus;
    public class OrderBilled: IEvent
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
        public double Amount { get; set; }
    }
}

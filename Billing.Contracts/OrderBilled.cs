namespace Billing.Contracts
{
    using NServiceBus;
    public class OrderBilled: IEvent
    {
        public string OrderId { get; set; }
    }
}

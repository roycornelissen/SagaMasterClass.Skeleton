namespace Sales.Contracts
{
    using NServiceBus;
    public class OrderCanceled: IEvent
    {
        public string OrderId { get; set; }
    }
}

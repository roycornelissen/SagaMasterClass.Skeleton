namespace Sales.Contracts
{
    using NServiceBus;
    public class OrderPlaced: IEvent
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
    }
}

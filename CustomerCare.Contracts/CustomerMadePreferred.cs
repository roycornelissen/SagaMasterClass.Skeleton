namespace CustomerCare.Contracts
{
    using NServiceBus;
    public class CustomerMadePreferred: IEvent
    {
        public string CustomerId { get; set; }
    }
}

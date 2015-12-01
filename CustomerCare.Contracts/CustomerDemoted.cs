namespace CustomerCare.Contracts
{
    using NServiceBus;
    public class CustomerDemoted: IEvent
    {
        public string CustomerId { get; set; }
    }
}

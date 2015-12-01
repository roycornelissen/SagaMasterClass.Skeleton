namespace Shipping.Contracts
{
    using NServiceBus;
    public class ShipFinalOrderResponse: IMessage
    {
        public bool Success { get; set; }
    }
}

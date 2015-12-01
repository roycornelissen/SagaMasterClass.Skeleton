namespace Shipping.Messages
{
    using NServiceBus;
    public class ShipOrderResponse: IMessage
    {
        public bool Success { get; set; }
    }
}

namespace Ups.Gateway
{
    using NServiceBus;
    using Shipping.Contracts;
    using Shipping.Contracts.Ups;

    public class ShipOrderHandler : IHandleMessages<ShipFinalOrderUps>
    {
        public IBus Bus { get; set; }

        public void Handle(ShipFinalOrderUps message)
        {
            var reply = new ShipFinalOrderResponse
            {
                Success = true
            };

            Bus.Reply(reply);
        }
    }
}


namespace Shipping.Contracts
{
    using NServiceBus;

    public class ShipFinalOrder: ICommand
    {
        public string Provider { get; set; }
        public string OrderId { get; set; }
    }
}


namespace Shipping.Contracts
{
    using NServiceBus;

    public class ShipFinalOrder : ICommand
    {
        public string OrderId { get; set; }
    }
}

namespace Shipping.Contracts.Ups
{
    public class ShipFinalOrderUps : ShipFinalOrder
    {

    }
}

namespace Shipping.Contracts.FedEx
{ 

    public class ShipFinalOrderFedex : ShipFinalOrder
    {
    }
}


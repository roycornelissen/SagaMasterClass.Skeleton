using System;

namespace Shipping
{
    using Contracts;
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;
    using NServiceBus.Timeout.Core;
    using ShipOrder = Messages.ShipOrder;

    public class ShipOrderPolicy: Saga<ShipOrderPolicy.SagaData>,
        IAmStartedByMessages<ShipOrder>,
        IHandleMessages<ShipFinalOrderResponse>,
        IHandleTimeouts<ShipOrderPolicy.RetryGatewayCall>
    {
        public class SagaData : ContainSagaData
        {
            [Unique]
            public virtual string OrderId { get; set; }
        }

        public class RetryGatewayCall : TimeoutData
        {
        }

        public void Handle(ShipOrder message)
        {
            Data.OrderId = message.OrderId;

            Console.Out.WriteLine($"Order {message.OrderId} is shipping");

            ShipOrder("FedEx");
        }

        void ShipOrder(string provider)
        {
            Bus.Send(new ShipFinalOrder
            {
                OrderId = Data.OrderId,
                Provider = provider
            });
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<ShipOrder>(m => m.OrderId).ToSaga(s => s.OrderId);
        }

        public void Handle(ShipFinalOrderResponse message)
        {
            if (message.Success)
            {
                ReplyToOriginator(new ShipOrderResponse
                {
                    Success = message.Success
                });
                MarkAsComplete();
            }
            else
            {
                RequestTimeout<RetryGatewayCall>(TimeSpan.FromSeconds(20));
            }
        }

        public void Timeout(RetryGatewayCall state)
        {
            ShipOrder("FedEx");
        }
    }
}

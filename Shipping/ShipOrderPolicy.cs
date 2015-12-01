using System;

namespace Shipping
{
    using Contracts;
    using Contracts.FedEx;
    using Contracts.Ups;
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;
    using NServiceBus.Timeout.Core;

    public class ShipOrderPolicy: Saga<ShipOrderPolicy.SagaData>,
        IAmStartedByMessages<ShipOrder>,
        IHandleMessages<ShipFinalOrderResponse>
    {
        public class SagaData : ContainSagaData
        {
            [Unique]
            public virtual string OrderId { get; set; }

            public virtual bool TriedFallback { get; set; }
        }

        public class RetryGatewayCall : TimeoutData
        {
        }

        public void Handle(ShipOrder message)
        {
            Data.OrderId = message.OrderId;
            Console.Out.WriteLine($"Order {message.OrderId} is shipping");
            ShipOrder(new ShipFinalOrderFedex());

            //todo: should set timeout for when FedEx never comes back (i.e. command ends up in error queue)
        }

        void ShipOrder(ShipFinalOrder command)
        {
            command.OrderId = Data.OrderId;
            Bus.Send(command);
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
                return;
            }

            if (Data.TriedFallback)
            {
                Data.TriedFallback = false;
                ShipOrder(new ShipFinalOrderFedex());
            }
            else
            {
                Data.TriedFallback = true;
                ShipOrder(new ShipFinalOrderUps());
            }
        }
    }
}

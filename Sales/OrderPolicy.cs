using System;

namespace Sales
{
    using Contracts;
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;
    using NServiceBus.Timeout.Core;

    public class OrderPolicy: Saga<OrderPolicy.OrderSagaData>, 
        IAmStartedByMessages<StartOrder>,
        IHandleMessages<PlaceOrder>,
        IHandleMessages<CancelOrder>,
        IHandleTimeouts<OrderPolicy.MarkSagaAsAbandoned>
    {
        public class OrderSagaData : ContainSagaData
        {
            [Unique]
            public virtual string OrderId { get; set; }
        }

        public class MarkSagaAsAbandoned : TimeoutData
        {
            
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<IOrderCommand>(m => m.OrderId).ToSaga(s => s.OrderId);
        }

        public void Handle(StartOrder message)
        {
            Data.OrderId = message.OrderId;

            var state = new MarkSagaAsAbandoned();
            
            RequestTimeout(TimeSpan.FromSeconds(20), state);
        }

        public void Handle(PlaceOrder message)
        {
            Bus.Publish(new OrderPlaced
            {
                OrderId = message.OrderId,
                CustomerId = message.OrderId
            });
            MarkAsComplete();
        }

        public void Handle(CancelOrder message)
        {
            Bus.Publish<OrderCanceled>(e => e.OrderId = message.OrderId);
            MarkAsComplete();
        }

        public void Timeout(MarkSagaAsAbandoned state)
        {
            Bus.Publish<OrderAbandoned>(e => e.OrderId = Data.OrderId);
            MarkAsComplete();
        }
    }
}

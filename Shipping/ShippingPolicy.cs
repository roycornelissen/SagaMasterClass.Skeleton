namespace Shipping
{
    using Billing.Contracts;
    using Messages;
    using NServiceBus.Saga;
    using Sales.Contracts;

    public class ShippingPolicy: Saga<ShippingPolicy.SagaData>,
        IAmStartedByMessages<OrderPlaced>,
        IAmStartedByMessages<OrderBilled>
    {
        public class SagaData : ContainSagaData
        {
            public virtual string OrderId { get; set; }
            public virtual  bool IsBilled { get; set; }
            public virtual bool IsPlaced { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<OrderPlaced>(e => e.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<OrderBilled>(e => e.OrderId).ToSaga(s => s.OrderId);
        }

        public void Handle(OrderPlaced message)
        {
            Data.OrderId = message.OrderId;
            Data.IsPlaced = true;

            CheckAndPublish();
        }

        void CheckAndPublish()
        {
            if (Data.IsPlaced && Data.IsBilled)
            {
                Bus.Send(new ShipOrder() { OrderId = Data.OrderId});
                MarkAsComplete();
            }
        }

        public void Handle(OrderBilled message)
        {
            Data.OrderId = message.OrderId;
            Data.IsBilled = true;

            CheckAndPublish();
        }
    }
}

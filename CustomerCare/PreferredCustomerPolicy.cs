namespace CustomerCare
{
    using System;
    using Billing.Contracts;
    using Contracts;
    using NServiceBus.Saga;

    public class PreferredCustomerPolicy: Saga<PreferredCustomerPolicy.SagaData>,
        IAmStartedByMessages<OrderBilled>,
        IHandleTimeouts<OrderBilled>
    {
        public class SagaData : ContainSagaData
        {
            [Unique]
            public virtual string CustomerId { get; set; }

            public virtual double TotalAmount { get; set; }

            public virtual bool IsPreferred { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<OrderBilled>(m => m.CustomerId).ToSaga(s => s.CustomerId);
        }

        public void Handle(OrderBilled message)
        {
            Data.CustomerId = message.CustomerId;

            Data.TotalAmount += message.Amount;

            if (Data.TotalAmount >= 5000d && !Data.IsPreferred)
            {
                Data.IsPreferred = true;
                Bus.Publish<CustomerMadePreferred>(e => e.CustomerId = message.CustomerId);
            }

            RequestTimeout(TimeSpan.FromSeconds(15), message);
        }

        public void Timeout(OrderBilled state)
        {
            Data.TotalAmount -= state.Amount;

            if (Data.TotalAmount < 5000d && Data.IsPreferred)
            {
                Data.IsPreferred = false;
                Bus.Publish<CustomerDemoted>(e => e.CustomerId = Data.CustomerId);
            }
        }
    }
}

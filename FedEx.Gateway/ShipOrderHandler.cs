namespace FedEx.Gateway
{
    using System;
    using System.Net.Http;
    using NServiceBus;
    using Shipping.Contracts;
    using Shipping.Contracts.FedEx;

    public class ShipOrderHandler: IHandleMessages<ShipFinalOrderFedex>
    {
        public IBus Bus { get; set; }

        public void Handle(ShipFinalOrderFedex message)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8888"),               
                Timeout = TimeSpan.FromSeconds(10)
            };

            var reply = new ShipFinalOrderResponse();
            using (var response = client.GetAsync("/fedex/shipit").Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            Bus.Reply(reply);
        }
    }
}

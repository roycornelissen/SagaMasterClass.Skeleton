namespace FedEx.Gateway
{
    using System;
    using System.Net.Http;
    using NServiceBus;
    using Shipping.Contracts;
    public class ShipOrderHandler: IHandleMessages<ShipFinalOrder>
    {
        public IBus Bus { get; set; }

        public void Handle(ShipFinalOrder message)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8888"),
                Timeout = TimeSpan.FromSeconds(10)
            };

            var reply = new ShipFinalOrderResponse();
            using (var response = client.GetAsync("/fedex/shipit").Result)
            {
                reply.Success = response.IsSuccessStatusCode;
                if (!response.IsSuccessStatusCode)
                {
                    reply.ErrorMessage = $"{response.StatusCode}: {response.ReasonPhrase}";
                }
            }

            Bus.Reply(reply);
        }
    }
}

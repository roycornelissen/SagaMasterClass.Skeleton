namespace Sales.Messages
{
    public class StartOrder : IOrderCommand
    {
        public string OrderId { get; set; }
    }
}
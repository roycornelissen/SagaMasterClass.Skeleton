namespace Sales.Messages
{
    public class CancelOrder : IOrderCommand
    {
        public string OrderId { get; set; }
    }
}
namespace Wdc.Sales.Orders.Api.Models
{

    public class CreateOrderInputModel
    {
        public List<OrderItemModel> Items { get; set; } = [];
    }

    public class CreateOrderOutPutModel
    {
        public required string Id { get; init; }
    }
}

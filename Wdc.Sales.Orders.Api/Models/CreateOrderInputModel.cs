namespace Wdc.Sales.Orders.Api.Models
{

    public class CreateOrderInputModel
    {
        public List<OrderItemModel> Items { get; set; } = [];
    }
}

namespace Wdc.Sales.Orders.Api.Models
{
    public class GetAllOrdersOutput
    {
        public IEnumerable<GetAllOrderOutPutModel> getAllOrderOutPutModels { get; init; } = [];
    }


    public class GetAllOrderOutPutModel
    {
        public required string OrderId { get; init; }
        public required string Status { get; init; }
        public required DateTime CreatedAt { get; init; }

        public IEnumerable<OrderItemOutPutModel> orderItemOutPutModels { get; init; } = [];
    }

    public class OrderItemOutPutModel
    {
        public required string ProductId { get; init; }
        public required int Quantity { get; init; }

    }
}

namespace Wdc.Sales.Orders.Api.Models
{
    public class GetAllOrdersOutput
    {
        public IEnumerable<GetAllOrdersOutputModel> getAllOrderOutPutModels { get; init; } = [];
    }


    public class GetAllOrdersOutputModel
    {
        public required string OrderId { get; init; }
        public required string Status { get; init; }
        public required DateTime CreatedAt { get; init; }

        public IEnumerable<OrderItemOutputModel> orderItemOutputModels { get; init; } = [];
    }

    public class OrderItemOutputModel
    {
        public required string ProductId { get; init; }
        public required int Quantity { get; init; }

    }
}

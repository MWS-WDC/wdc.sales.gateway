namespace Wdc.Sales.Orders.Api.Models
{

    public class AddProductInputModel
    {
        private AddProductInputModel() { }

        public required string Id { get; init; }
        public required long Quantity { get; init; }
        public required decimal Price { get; init; }
    }
}

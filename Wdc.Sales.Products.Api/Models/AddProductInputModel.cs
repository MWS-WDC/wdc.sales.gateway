namespace Wdc.Sales.Products.Api.Models
{

    public class AddProductInputModel
    {
        public required string Id { get; init; }
        public required long Quantity { get; init; }
        public required decimal Price { get; init; }
    }
}

namespace Wdc.Sales.Products.Api.Models
{
    public class AddProductInputModel
    {
        public string ProductId { get; set; } = string.Empty;
        public long Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

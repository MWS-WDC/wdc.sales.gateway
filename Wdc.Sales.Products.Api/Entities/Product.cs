namespace Wdc.Sales.Products.Api.Entities
{
    public class Product
    {
        private Product(
            string id,
            long quantity,
            decimal price,
            int sequence
        )
        {
            Id = id;
            Quantity = quantity;
            Price = price;
            Sequence = sequence;
        }

        public string Id { get; private set; }
        public long Quantity { get; private set; }
        public decimal Price { get; private set; }
        public int Sequence { get; private set; }

        public static Product Add(
            string id,
            long quantity,
            decimal price
        ) => new(
            id: id,
            quantity: quantity,
            price: price,
            sequence: 1
        );

        public void UpdateQuantity(long quantiy) => Quantity -= quantiy;

        public void UpdateSequence(int sequence)
            => Sequence = sequence;
    }
}

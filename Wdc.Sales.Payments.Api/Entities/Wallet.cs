namespace Wdc.Sales.Payments.Api.Entities
{
    public class Wallet
    {
        private Wallet(
            Guid ownerId,
            decimal balance,
            string locationName,
            int sequence
        )
        {
            OwnerId = ownerId;
            Balance = balance;
            LocationName = locationName;
            Sequence = sequence;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid OwnerId { get; private set; }
        public decimal Balance { get; private set; }
        public string LocationName { get; private set; }
        public int Sequence { get; private set; }

        public static Wallet Create(
            Guid ownerId,
            decimal balance,
            string locationName
        ) => new(
            ownerId: ownerId,
            balance: balance,
            locationName: locationName,
            sequence: 1
        );

        public void ReduceBalance(decimal balance, string locationName)
        {
            Sequence++;
            Balance -= balance;
            LocationName = locationName;
        }
    }
}

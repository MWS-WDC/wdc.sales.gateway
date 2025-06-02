namespace Wdc.Sales.Payments.Api.Entities
{
    public class Wallet
    {
        private Wallet(
            Guid ownerId,
            decimal balance,
            int sequence
        )
        {
            OwnerId = ownerId;
            Balance = balance;
            Sequence = sequence;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid OwnerId { get; private set; }
        public decimal Balance { get; private set; }
        public int Sequence { get; private set; }

        public static Wallet Create(
            Guid ownerId,
            decimal balance
        ) => new(
            ownerId: ownerId,
            balance: balance,
            sequence: 1
        );

        public void ReduceBalance(decimal balance)
        {
            Sequence++;
            Balance -= balance;
        }
    }
}

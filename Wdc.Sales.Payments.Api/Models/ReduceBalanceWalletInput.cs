namespace Wdc.Sales.Payments.Api.Models
{
    public class ReduceBalanceWalletInput
    {
        public required string LocationName { get; init; }
        public string OrderId { get; init; } = string.Empty;
        public List<ReduceBalanceWallet> ReduceBalanceWallets { get; init; } = [];
    }

    public class ReduceBalanceWallet
    {
        public required string ProductId { get; init; }
        public required long Quantity { get; init; }
    }
}

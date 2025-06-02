using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wdc.Sales.Payments.Api.Entities;

namespace Wdc.Sales.Payments.Api.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.Property(x => x.Sequence).IsConcurrencyToken();

            builder.Property(x => x.Balance).HasPrecision(18, 3);

            builder.HasIndex(x => x.OwnerId).IsUnique();
        }
    }
}

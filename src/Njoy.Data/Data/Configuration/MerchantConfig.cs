using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Njoy.Data
{
    public sealed class MerchantConfig : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.ToTable("Merchants").HasKey(m => m.Id);
            builder.HasOne(m => m.User).WithOne().HasForeignKey<Merchant>((m) => m.UserId);
            builder.HasMany(m => m.BusinessMerchants);
        }
    }
}
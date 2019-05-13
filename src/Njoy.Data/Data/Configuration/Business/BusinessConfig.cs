using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Njoy.Data
{
    public sealed class BusinessConfig : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
            builder.ToTable("Businesses").HasKey(e => e.Id);
            builder.HasMany(e => e.BusinessMerchants);
            builder.HasOne(e => e.Address);
        }
    }
}
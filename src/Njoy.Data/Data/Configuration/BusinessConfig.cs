using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Njoy.Data
{
    public sealed class BusinessConfig : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
            builder.ToTable("Businesses").HasKey(b => b.Id);
            builder.HasIndex(b => b.Code).IsUnique();
            builder.HasMany(b => b.BusinessMerchants);
        }
    }
}
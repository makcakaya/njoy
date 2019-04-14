using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Njoy.Data
{
    public sealed class BusinessMerchantConfig : IEntityTypeConfiguration<BusinessMerchant>
    {
        public void Configure(EntityTypeBuilder<BusinessMerchant> builder)
        {
            builder.ToTable("BusinessMerchants").HasKey(b => b.Id);
            builder.HasOne(bm => bm.Business).WithMany().HasForeignKey(bm => bm.BusinessId).IsRequired();
            builder.HasOne(bm => bm.Merchant).WithMany().HasForeignKey(bm => bm.MerchantId).IsRequired();
        }
    }
}
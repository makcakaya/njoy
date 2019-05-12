using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Njoy.Data
{
    public sealed class BusinessAddressConfig : IEntityTypeConfiguration<BusinessAddress>
    {
        public void Configure(EntityTypeBuilder<BusinessAddress> builder)
        {
            builder.ToTable("BusinessAddresses");
            builder.HasOne(e => e.Business).WithOne(ee => ee.Address).HasForeignKey<BusinessAddress>(e => e.BusinessId);
            builder.HasOne(e => e.District).WithMany().HasForeignKey(e => e.DistrictId);
        }
    }
}
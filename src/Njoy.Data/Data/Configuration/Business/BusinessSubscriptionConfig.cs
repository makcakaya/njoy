using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Njoy.Data
{
    public sealed class BusinessSubscriptionConfig : IEntityTypeConfiguration<BusinessSubscription>
    {
        public void Configure(EntityTypeBuilder<BusinessSubscription> builder)
        {
            builder.ToTable("BusinessSubscriptions");
            builder.HasKey(b => b.Id);
            builder.HasOne(b => b.Business).WithMany().HasForeignKey(b => b.BusinessId).IsRequired();
            builder.Property(b => b.CreateDate).IsRequired();
            builder.Property(b => b.State).IsRequired();
        }
    }
}
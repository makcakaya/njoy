using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Njoy.Data
{
    public sealed class DistrictConfig : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToTable("Districts");
            builder.Property(e => e.Name).IsRequired();
            builder.HasOne(e => e.County).WithMany().HasForeignKey(e => e.CountyId);
            builder.HasAlternateKey(e => new { e.CountyId, e.Name });
        }
    }
}
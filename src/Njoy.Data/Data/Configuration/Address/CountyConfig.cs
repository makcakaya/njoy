using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Njoy.Data
{
    public sealed class CountyConfig : IEntityTypeConfiguration<County>
    {
        public void Configure(EntityTypeBuilder<County> builder)
        {
            builder.ToTable("Counties");
            builder.Property(e => e.Name).IsRequired();
            builder.HasOne(e => e.City).WithMany().HasForeignKey(e => e.CityId);
        }
    }
}
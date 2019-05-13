using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Njoy.Data
{
    public sealed class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.LicensePlateCode).IsRequired();
        }
    }
}
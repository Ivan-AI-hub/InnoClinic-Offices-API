using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficesAPI.Domain;

namespace OfficesAPI.DAL.EntityConfiguration
{
    internal class OfficeConfigurator : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasKey(o => o.Id);
            builder.OwnsOne(o => o.Address);

            builder.HasIndex(o => new { o.Address.City, o.OfficeNumber }).IsUnique(true);

            builder.Property(o => o.OfficeNumber).IsRequired();
            builder.Property(o => o.Address).IsRequired();
            builder.Property(o => o.PhoneNumber).IsRequired();
            builder.Property(o => o.Status).IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalManagement.Repository.Models.Configurations
{
    public class ServiceRecordConfiguration : IEntityTypeConfiguration<ServiceRecord>
    {
        public void Configure(EntityTypeBuilder<ServiceRecord> builder)
        {
            builder.HasKey(sr => sr.Id);

            builder.Property(sr => sr.Cost)
                   .HasColumnType("decimal(18,2)");
        }
    }
}

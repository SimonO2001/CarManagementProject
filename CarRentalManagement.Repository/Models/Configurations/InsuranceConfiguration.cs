using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalManagement.Repository.Models.Configurations
{
    public class InsuranceConfiguration : IEntityTypeConfiguration<Insurance>
    {
        public void Configure(EntityTypeBuilder<Insurance> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Provider).IsRequired().HasMaxLength(100);
            builder.Property(i => i.PolicyNumber).IsRequired().HasMaxLength(50);
            builder.Property(i => i.Coverage).IsRequired();
            builder.Property(i => i.CostADay).HasColumnType("decimal(18,2)");

            builder.HasOne(i => i.Vehicle)
                   .WithMany(v => v.Insurances)
                   .HasForeignKey(i => i.VehicleId)
                   .OnDelete(DeleteBehavior.Restrict);  // Specify Restrict to avoid cycles or multiple cascade paths
        }
    }
}

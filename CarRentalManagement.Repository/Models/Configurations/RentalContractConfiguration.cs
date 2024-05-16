using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRentalManagement.Repository.Models.Configurations
{
    public class RentalContractConfiguration : IEntityTypeConfiguration<RentalContract>
    {
        public void Configure(EntityTypeBuilder<RentalContract> builder)
        {
            builder.HasKey(rc => rc.Id);

            builder.Property(rc => rc.TotalCost).HasColumnType("decimal(18,2)");

            builder.HasOne(rc => rc.Vehicle)
                   .WithMany()
                   .HasForeignKey(rc => rc.VehicleId)
                   .OnDelete(DeleteBehavior.Restrict); // Specify Restrict to avoid cycles or multiple cascade paths

            builder.HasOne(rc => rc.Customer)
                   .WithMany()
                   .HasForeignKey(rc => rc.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict); // Specify Restrict to avoid cycles or multiple cascade paths

            builder.HasOne(rc => rc.Insurance)
                   .WithMany()
                   .HasForeignKey(rc => rc.InsuranceId)
                   .OnDelete(DeleteBehavior.Restrict); // Specify Restrict to avoid cycles or multiple cascade paths
        }
    }
}

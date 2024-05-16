using CarRentalManagement.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagement.Repository.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<RentalContract> RentalContracts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ServiceRecord> ServiceRecords { get; set; }
        public DbSet<Insurance> Insurances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}

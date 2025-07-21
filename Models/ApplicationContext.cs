using Microsoft.EntityFrameworkCore;

namespace VitalSignsMonitor.Models
{
 
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<VitalSign> VitalSigns { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships
            modelBuilder.Entity<VitalSign>()
                .HasOne(v => v.Patient)
                .WithMany(p => p.VitalSigns)
                .HasForeignKey(v => v.PatientId);

            // Data Seeding
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, Name = "John Doe", Age = 45, RoomNumber = "101" },
                new Patient { Id = 2, Name = "Jane Smith", Age = 32, RoomNumber = "102" },
                new Patient { Id = 3, Name = "Bob Johnson", Age = 67, RoomNumber = "103" }
            );
        }
    }   
}

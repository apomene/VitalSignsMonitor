using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VitalSignsMonitor.Models
{
    public class ApplicationDbContext : IdentityDbContext 
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

            // Seeding
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, Name = "John Doe", Age = 45, RoomNumber = "101" },
                new Patient { Id = 2, Name = "Jane Smith", Age = 32, RoomNumber = "102" },
                new Patient { Id = 3, Name = "Bob Johnson", Age = 67, RoomNumber = "103" }
            );

            modelBuilder.Entity<VitalSign>().HasData(
                new VitalSign { Id = 1, PatientId = 1, HeartRate = 80, SystolicBP = 120, DiastolicBP = 80, OxygenSaturation = 97, Timestamp = DateTime.Parse("2025-07-21T12:00:00") },
                new VitalSign { Id = 2, PatientId = 2, HeartRate = 95, SystolicBP = 135, DiastolicBP = 85, OxygenSaturation = 92, Timestamp = DateTime.Parse("2025-07-21T12:30:00") },
                new VitalSign { Id = 3, PatientId = 3, HeartRate = 110, SystolicBP = 145, DiastolicBP = 95, OxygenSaturation = 88, Timestamp = DateTime.Parse("2025-07-21T13:00:00") }
            );
        }
    }
}

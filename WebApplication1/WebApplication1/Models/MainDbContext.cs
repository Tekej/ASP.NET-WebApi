using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext()
        {
        }

        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LoginRequest> LoginRequest { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Medicament> Medicament { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicament { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Doctors");
            modelBuilder.Entity<LoginRequest>(entity =>
            {
                entity.HasKey(e => e.IdUser);
                entity.Property(e => e.IdUser).ValueGeneratedOnAdd();
                entity.Property(e => e.Login).IsRequired();
                entity.Property(e => e.Password).IsRequired();

                entity.ToTable("LoginRequest");

                entity.HasMany(l => l.AppUser)
                    .WithOne(a => a.LoginRequest)
                    .HasForeignKey(a => a.IdUser)
                    .IsRequired();
            });
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasKey(e => e.IdToken);
                entity.Property(e => e.IdToken).ValueGeneratedOnAdd();
                entity.Property(e => e.IdUser).IsRequired();

                entity.ToTable("AppUser");

            });
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.IdDoctor);
                entity.Property(e => e.IdDoctor).ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).IsRequired();

                entity.ToTable("Doctor");

                entity.HasMany(d => d.Prescription)
                    .WithOne(p => p.Doctor)
                    .HasForeignKey(p => p.IdDoctor)
                    .IsRequired();
            });
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.HasKey(e => e.IdPrescription);
                entity.Property(e => e.IdPrescription).ValueGeneratedOnAdd();
                entity.ToTable("Prescription");

                entity.HasMany(d => d.PrescriptionMedicament)
                    .WithOne(p => p.Prescription)
                    .HasForeignKey(p => p.IdPrescription)
                    .IsRequired();
            });
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.IdPatient);
                entity.Property(e => e.IdPatient).ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).IsRequired();
                entity.ToTable("Patient");

                entity.HasMany(d => d.Prescription)
                    .WithOne(p => p.Patient)
                    .HasForeignKey(p => p.IdPatient)
                    .IsRequired();
            });
            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(e => e.IdMedicament);
                entity.Property(e => e.IdMedicament).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.ToTable("Medicament");


                entity.HasMany(d => d.PrescriptionMedicament)
                    .WithOne(p => p.Medicament)
                    .HasForeignKey(p => p.IdMedicament)
                    .IsRequired();
            });
            modelBuilder.Entity<PrescriptionMedicament>(entity =>
            {
                entity.HasKey(e => new {e.IdMedicament, e.IdPrescription});
                entity.Property(e => e.Dose).IsRequired();
                entity.ToTable("PrescriptionMedicament");
            });

            modelBuilder.Seed();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(
                "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s19374;Integrated Security=True");
        }
    }
}
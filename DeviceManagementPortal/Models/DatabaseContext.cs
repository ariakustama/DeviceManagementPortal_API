using Microsoft.EntityFrameworkCore;

namespace DeviceManagementPortal.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { 

        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { 
        
        }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Backend> Backends { get; set; }
        public virtual DbSet<DeviceBackend> DeviceBackends { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.IMEI)
                    .HasMaxLength(20)
                    .IsUnicode(true);

                entity.Property(e => e.Model)
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.SimCardNo)
                   .HasMaxLength(20)
                   .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.CreatedBy)
                   .HasMaxLength(20)
                   .IsUnicode(false);
            });

            modelBuilder.Entity<Backend>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                   .HasMaxLength(50)
                   .IsUnicode(false);
            });

            modelBuilder.Entity<DeviceBackend>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.IdDevice)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.IdBackEnd)
                   .HasMaxLength(36)
                   .IsUnicode(false);

                entity.Property(e => e.MappedTime).HasDefaultValueSql("(sysdatetime())");
            });
        }
    }
}

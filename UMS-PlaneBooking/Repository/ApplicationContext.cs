using Microsoft.EntityFrameworkCore;
using UMS_PlaneBooking.Repository.Entities;

namespace UMS_PlaneBooking.Repository;

public class ApplicationContext : DbContext
{
    #region data sets declarations
    public DbSet<AirportEntity> Airports { get; set; }
    public DbSet<FlightsEntity> Flights { get; set; }
    public DbSet<PlaneEntity> Planes { get; set; }
    public DbSet<PassengerEntity> Passengers { get; set; }
    #endregion
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure AirportEntity
        modelBuilder.Entity<AirportEntity>(entity =>
        {
            entity.HasKey(a => a.AirportID);
            entity.Property(a => a.AirportName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Address).HasMaxLength(200);

            // One-to-many relationship with FlightsEntity
            entity.HasMany(a => a.Flights)
                  .WithOne(f => f.Airport)
                  .HasForeignKey(f => f.AirportID)
                  .OnDelete(DeleteBehavior.Cascade); // Delete flights when airport is deleted
        });

        // Configure PlaneEntity
        modelBuilder.Entity<PlaneEntity>(entity =>
        {
            entity.HasKey(p => p.PlaneID);
            entity.Property(p => p.PlaneCode).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Airline).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Model).HasMaxLength(100);

            // One-to-many relationship with FlightsEntity
            entity.HasMany(p => p.Flights)
                  .WithOne(f => f.Plane)
                  .HasForeignKey(f => f.PlaneID)
                  .OnDelete(DeleteBehavior.Cascade); // Delete flights when plane is deleted
        });

        // Configure PassengerEntity
        modelBuilder.Entity<PassengerEntity>(entity =>
        {
            entity.HasKey(p => p.BookingID);
            entity.Property(p => p.PassengerName).IsRequired().HasMaxLength(100);

            // Many-to-one relationship with FlightsEntity
            entity.HasOne(p => p.Flight)
                  .WithMany(f => f.Passengers)
                  .HasForeignKey(p => p.FlightID)
                  .OnDelete(DeleteBehavior.Cascade); // Delete passengers when flight is deleted
        });

        // Configure FlightsEntity
        modelBuilder.Entity<FlightsEntity>(entity =>
        {
            entity.HasKey(f => f.FlightID);
            entity.Property(f => f.FlightCode).IsRequired().HasMaxLength(50);

            // Relationship with AirportEntity (many-to-one)
            entity.HasOne(f => f.Airport)
                  .WithMany(a => a.Flights)
                  .HasForeignKey(f => f.AirportID)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relationship with PlaneEntity (many-to-one)
            entity.HasOne(f => f.Plane)
                  .WithMany(p => p.Flights)
                  .HasForeignKey(f => f.PlaneID)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relationship with PassengerEntity (one-to-many)
            entity.HasMany(f => f.Passengers)
                  .WithOne(p => p.Flight)
                  .HasForeignKey(p => p.FlightID)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentDate = DateTime.Now;
        var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

        foreach (var entity in addedEntities)
        {
            if (entity.Properties.Any(p => p.Metadata.Name == "CreatedOn"))
            {
                entity.Property("CreatedOn").CurrentValue = currentDate;
            }
        }

        var updatedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
        foreach (var entity in updatedEntities)
        {
            if (entity.Properties.Any(p => p.Metadata.Name == "ChangedOn"))
            {
                entity.Property("ChangedOn").CurrentValue = currentDate;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

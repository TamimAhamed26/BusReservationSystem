using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Bus> Buses => Set<Bus>();
        public DbSet<Route> Routes => Set<Route>();
        public DbSet<BusSchedule> BusSchedules => Set<BusSchedule>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Passenger> Passengers => Set<Passenger>();
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-Many: Bus -> BusSchedule
            modelBuilder.Entity<Bus>()
                .HasMany(b => b.Schedules)
                .WithOne(s => s.Bus)
                .HasForeignKey(s => s.BusId);

            // One-to-Many: Route -> BusSchedule
            modelBuilder.Entity<Route>()
                .HasMany(r => r.BusSchedules)
                .WithOne(s => s.Route)
                .HasForeignKey(s => s.RouteId);

            // One-to-Many: BusSchedule -> Seat
            modelBuilder.Entity<BusSchedule>()
                .HasMany(s => s.Seats)
                .WithOne(se => se.BusSchedule)
                .HasForeignKey(se => se.BusScheduleId);
            
            // One-to-Many: Passenger -> Ticket
            modelBuilder.Entity<Passenger>()
                .HasMany(p => p.Tickets)
                .WithOne(t => t.Passenger)
                .HasForeignKey(t => t.PassengerId);

            // One-to-One: Seat -> Ticket
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Ticket)
                .WithOne(t => t.Seat)
                .HasForeignKey<Ticket>(t => t.SeatId);
        }
    }
}
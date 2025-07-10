using Microsoft.EntityFrameworkCore;
using ST10434048_BookingSystem.Models.Entities;
using ST10434048_BookingSystem.Models.View_Models;

namespace ST10434048_BookingSystem.Data
{
    public class BookingsDbContext : DbContext
    {
        public BookingsDbContext(DbContextOptions<BookingsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetailsViewmodel> BookingDetailsViewmodel { get; set; }
        public DbSet<EventType> EventTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between Venue and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the one-to-many relationship between Event and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the one-to-many relationship between EventType and Event
            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(et => et.Events)
                .HasForeignKey(e => e.EventTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>().HasIndex(e => e.BookingId).IsUnique();

            modelBuilder.Entity<BookingDetailsViewmodel>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("BookingDetailsVW");
            });
        }
    }
}
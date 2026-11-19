using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Persistence.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(b => b.Id);
        // The Id is generated in the domain, not by the database.
        builder.Property(b => b.Id).ValueGeneratedNever();
        builder.Property(b => b.BookedBy).IsRequired();
        builder.Property(b => b.StartUtc).HasConversion<UtcDateTimeConverter>();
        builder.Property(b => b.EndUtc).HasConversion<UtcDateTimeConverter>();
        builder.Property(b => b.Attendees);
        // Enums are stored as text, so the database stays readable.
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasOne<Room>().WithMany().HasForeignKey(b => b.RoomId);
    }
}

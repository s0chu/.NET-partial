using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence.Repositories;

public sealed class BookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken) =>
        await db.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Booking>> GetActiveForParticipantOnDayAsync(
        string bookedBy, DateTime dayUtc, CancellationToken cancellationToken)
    {
        var dayStart = dayUtc.Date;
        var nextDay = dayStart.AddDays(1);

        return await db.Bookings
            .Where(b => b.BookedBy == bookedBy &&
                        b.Status == BookingStatus.Active &&
                        b.StartUtc >= dayStart &&
                        b.StartUtc < nextDay)
            .ToListAsync(cancellationToken);
    }

    public void Add(Booking booking) => db.Bookings.Add(booking);
}

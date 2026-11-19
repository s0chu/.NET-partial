namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken);

    // Active bookings of bookedBy, in any room, whose StartUtc falls on the UTC day of dayUtc.
    Task<IReadOnlyList<Booking>> GetActiveForParticipantOnDayAsync(
        string bookedBy, DateTime dayUtc, CancellationToken cancellationToken);

    // Only marks the booking as new; IUnitOfWork saves it.
    void Add(Booking booking);
}

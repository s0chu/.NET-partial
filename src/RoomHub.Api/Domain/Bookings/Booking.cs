using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    private static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

    private Booking()
    {
    }

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    // nowUtc comes from the caller's TimeProvider; the domain never reads the clock.
    public static Result<Booking> Create(
        Room room, string? bookedBy, DateTime startUtc, DateTime endUtc, int attendees, DateTime nowUtc)
    {
        startUtc = ToUtc(startUtc);
        endUtc = ToUtc(endUtc);

        // R2
        if (string.IsNullOrWhiteSpace(bookedBy))
            return BookingErrors.InvalidBookedBy;

        // R3
        if (endUtc <= startUtc)
            return BookingErrors.InvalidRange;

        // R4
        if (attendees <= 0)
            return BookingErrors.InvalidAttendees;

        if (attendees > room.Capacity)
            return BookingErrors.AttendeesExceedCapacity;

        // R5
        if (startUtc < nowUtc)
            return BookingErrors.StartInPast;

        // R6: exactly MaxDuration is allowed.
        if (endUtc - startUtc > MaxDuration)
            return BookingErrors.TooLong;

        return new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            BookedBy = bookedBy.Trim(),
            StartUtc = startUtc,
            EndUtc = endUtc,
            Attendees = attendees,
            Status = BookingStatus.Active
        };
    }

    // R7: intervals are half-open, so bookings that only touch do not overlap,
    // and a cancelled booking never blocks anything.
    public bool OverlapsWith(Booking other) =>
        Status == BookingStatus.Active &&
        other.Status == BookingStatus.Active &&
        StartUtc < other.EndUtc &&
        other.StartUtc < EndUtc;

    // R8
    public Result<Unit> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return BookingErrors.AlreadyCancelled;

        Status = BookingStatus.Cancelled;
        return Unit.Value;
    }

    // The contract says dates are UTC: a value without an offset is taken as UTC, one with an offset is converted.
    private static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}

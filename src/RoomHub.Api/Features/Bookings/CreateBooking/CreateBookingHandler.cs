using RoomHub.Api.Common;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(RoomRepository rooms, BookingRepository bookings)
{
    public Booking Handle(Guid roomId, string? bookedBy, DateTime startUtc, DateTime endUtc, int attendees)
    {
        var room = rooms.Get(roomId);
        if (room is null)
            throw new BusinessException("RoomNotFound", "The room does not exist.", 404);

        if (string.IsNullOrWhiteSpace(bookedBy))
            throw new BusinessException("InvalidBookedBy", "BookedBy is required.", 400);

        if (endUtc <= startUtc)
            throw new BusinessException("InvalidRange", "The end must be after the start.", 400);

        if (attendees <= 0)
            throw new BusinessException("InvalidAttendees", "Attendees must be positive.", 400);

        if (attendees > room.Capacity)
            throw new BusinessException("AttendeesExceedCapacity", "The room does not have enough seats.", 400);

        foreach (var existing in bookings.ListForRoom(roomId))
        {
            if (existing.StartUtc <= endUtc && startUtc <= existing.EndUtc)
                throw new BusinessException("RoomOccupied", "The room is occupied in this interval.", 409);
        }

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            RoomId = roomId,
            BookedBy = bookedBy.Trim(),
            StartUtc = startUtc,
            EndUtc = endUtc,
            Attendees = attendees,
            Status = BookingStatus.Active
        };

        bookings.Add(booking);
        return booking;
    }
}

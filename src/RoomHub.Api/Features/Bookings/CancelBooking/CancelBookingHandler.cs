using RoomHub.Api.Common;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(BookingRepository bookings)
{
    public void Handle(Guid id)
    {
        var booking = bookings.Get(id);
        if (booking is null)
            throw new BusinessException("BookingNotFound", "The booking does not exist.", 404);

        booking.Status = BookingStatus.Cancelled;
    }
}

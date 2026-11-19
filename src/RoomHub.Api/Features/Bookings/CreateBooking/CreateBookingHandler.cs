using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid RoomId, string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees)
    : ICommand<CreateBookingResponse>;

public sealed class CreateBookingHandler(RoomRepository rooms, BookingRepository bookings, TimeProvider time)
    : ICommandHandler<CreateBookingCommand, CreateBookingResponse>
{
    public async Task<Result<CreateBookingResponse>> Handle(
        CreateBookingCommand command, CancellationToken cancellationToken)
    {
        // The room is loaded first, so a missing room is a 404 before any validation.
        var room = await rooms.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
            return RoomErrors.NotFound;

        var result = Booking.Create(
            room, command.BookedBy, command.StartUtc, command.EndUtc, command.Attendees,
            time.GetUtcNow().UtcDateTime);
        if (result.IsFailure)
            return result.Error;

        var booking = result.Value;
        foreach (var existing in await bookings.ListForRoomAsync(room.Id, cancellationToken))
        {
            if (existing.StartUtc <= booking.EndUtc && booking.StartUtc <= existing.EndUtc)
                return BookingErrors.RoomOccupied;
        }

        bookings.Add(booking);
        return new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status);
    }
}

using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingCommand(
    Guid RoomId, string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees)
    : ICommand<CreateBookingResponse>;

public sealed class CreateBookingHandler(
    IRoomRepository rooms, IBookingRepository bookings, IUnitOfWork unitOfWork, TimeProvider time)
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
        var active = await bookings.GetActiveForRoomAsync(room.Id, cancellationToken);
        if (active.Any(booking.OverlapsWith))
            return BookingErrors.RoomOccupied;

        bookings.Add(booking);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status);
    }
}

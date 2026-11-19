using Microsoft.AspNetCore.Http.HttpResults;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingRequest(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

public sealed record CreateBookingResponse(Guid Id, Guid RoomId, string Status);

public static class CreateBookingEndpoint
{
    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/{roomId:guid}/bookings", Handle)
            .WithName("CreateBooking");

    private static Created<CreateBookingResponse> Handle(
        Guid roomId, CreateBookingRequest request, CreateBookingHandler handler)
    {
        var booking = handler.Handle(
            roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees);

        return TypedResults.Created(
            $"/api/bookings/{booking.Id}",
            new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString()));
    }
}

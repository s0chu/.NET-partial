using Microsoft.AspNetCore.Http.HttpResults;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed record GetBookingResponse(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    string Status);

public static class GetBookingEndpoint
{
    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetBooking");

    private static Ok<GetBookingResponse> Handle(Guid id, GetBookingHandler handler)
    {
        var b = handler.Handle(id);
        return TypedResults.Ok(new GetBookingResponse(
            b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status.ToString()));
    }
}

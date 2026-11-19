using Microsoft.AspNetCore.Http.HttpResults;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/{id:guid}/cancel", Handle)
            .WithName("CancelBooking");

    private static NoContent Handle(Guid id, CancelBookingHandler handler)
    {
        handler.Handle(id);
        return TypedResults.NoContent();
    }
}

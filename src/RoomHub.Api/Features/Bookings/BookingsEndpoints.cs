using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;

namespace RoomHub.Api.Features.Bookings;

public static class BookingsEndpoints
{
    public static IEndpointRouteBuilder MapBookings(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/bookings").WithTags("Bookings");
        GetBookingEndpoint.Map(group);
        CancelBookingEndpoint.Map(group);

        // Creating a booking is a Bookings use case, but its route lives under /api/rooms/{roomId}.
        var roomGroup = app.MapGroup("/api/rooms").WithTags("Bookings");
        CreateBookingEndpoint.Map(roomGroup);

        return app;
    }
}

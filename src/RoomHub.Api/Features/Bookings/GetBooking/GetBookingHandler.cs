using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid BookingId) : IQuery<GetBookingResponse>;

public sealed class GetBookingHandler(IBookingRepository bookings)
    : IQueryHandler<GetBookingQuery, GetBookingResponse>
{
    public async Task<Result<GetBookingResponse>> Handle(GetBookingQuery query, CancellationToken cancellationToken)
    {
        var b = await bookings.GetByIdAsync(query.BookingId, cancellationToken);
        if (b is null)
            return BookingErrors.NotFound;

        return new GetBookingResponse(b.Id, b.RoomId, b.BookedBy, b.StartUtc, b.EndUtc, b.Attendees, b.Status);
    }
}

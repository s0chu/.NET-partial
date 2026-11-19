using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error InvalidBookedBy =
        Error.Validation("InvalidBookedBy", "BookedBy is required.");

    public static readonly Error InvalidRange =
        Error.Validation("InvalidRange", "The end must be after the start.");

    public static readonly Error InvalidAttendees =
        Error.Validation("InvalidAttendees", "Attendees must be positive.");

    public static readonly Error AttendeesExceedCapacity =
        Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats.");

    public static readonly Error StartInPast =
        Error.Validation("StartInPast", "The booking cannot start in the past.");

    public static readonly Error TooLong =
        Error.Validation("BookingTooLong", "A booking can last at most 4 hours.");

    public static readonly Error TooLongForLargeRoom =
        Error.Validation("BookingTooLongForLargeRoom", "Rooms with more than 10 seats can be booked for at most 2 hours.");

    public static readonly Error RoomOccupied =
        Error.Conflict("RoomOccupied", "The room is occupied in this interval.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled.");

    public static readonly Error TooLateToCancel =
        Error.Conflict("TooLateToCancel", "A booking can be cancelled only if it starts at least 60 minutes from now.");

    public static readonly Error DailyLimitReached =
        Error.Conflict("DailyBookingLimitReached", "A participant cannot have more than 2 active bookings on the same day.");

    public static readonly Error NotFound =
        Error.NotFound("BookingNotFound", "The booking does not exist.");
}

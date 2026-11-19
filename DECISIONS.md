# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

R3: 
starter -> src/RoomHub.Api/Services/BookingService.cs, line 17. 
Create method in that service checked "if (endUtc <= startUtc)" and threw a BusinessException with status 400 -> middleware then caught it and turned it into an error response
Now -> src/RoomHub.Api/Domain/Bookings/Booking.cs, line 38. The check is the same, but it now returns BookingErrors.InvalidRange instead of throwing.
The rule is about what a valid booking is, so it belongs to the Booking class itself and not to a service or an endpoint. (Domain rules)
The handler and the endpoint no longer need to know about the rule. They just call Booking.Create and pass the result on.

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

Result is only an object, compared with an exception which is far more cost expensive (stack trace). Using Result pattern you can also check the error in a safe manner, non disruptive

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

EF Core aware (Infrastructure/Persistence + Program.cs):
RoomHubDbContext (also implements IUnitOfWork), RoomConfiguration, BookingConfiguration,
UtcDateTimeConverter, RoomRepository, BookingRepository, DatabaseInitializer, Program.cs (UseSqlite).

Not EF Core aware:
Domain: Room, Booking, BookingStatus, RoomErrors, BookingErrors, Result, Error, ErrorType, Unit,
IRoomRepository, IBookingRepository, IUnitOfWork.
Features: all handlers and endpoints (they only see the domain interfaces).
Common: CQRS interfaces, ErrorHttpExtensions, ExceptionMiddleware.

Gain:
Handlers can be tested with fake repositories, no database needed.
One SaveChangesAsync per use case, so changes are saved together.
All queries sit in the two repository classes.
The database can be replaced without touching Domain or Features.

Pay:
More code: 3 interfaces, 2 classes, DI registrations.
Some repository methods only forward calls to EF.
Repositories and Unit of Work must share one Scoped DbContext.

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

POST /api/rooms/{roomId}/bookings -> ExceptionMiddleware -> BookingsEndpoints.MapBookings -> CreateBookingEndpoint.Handle (CreateBookingCommand) -> CreateBookingHandler.Handle -> IRoomRepository/RoomRepository.GetByIdAsync (404) -> Booking.Create with TimeProvider (400: R2-R6, R10) -> IBookingRepository/BookingRepository.GetActiveForRoomAsync + Booking.OverlapsWith (409 R7) -> GetActiveForParticipantOnDayAsync + Booking.WouldExceedDailyLimit (409 R9) -> BookingRepository.Add -> IUnitOfWork/RoomHubDbContext.SaveChangesAsync -> INSERT Bookings in roomhub.db -> Result -> ToProblem (400/404/409) or TypedResults.Created (201)


## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

_(your answer here)_

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

Risks left: Concurrency: add a version token on Room, or wrap the check and the insert in a transaction, so the second request gets a 409.
Improve: Concurrency -> add a version token on Room, so the second request gets a 409.

# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| _(fill in)_ | _(fill in)_ | _(fill in)_ | _(fill in)_ |
|3 min | you are in ./src. read ../ASSIGNMENT.md, ../README.md, ../roomhub.http and all code in RoomHub.Api. the contract and R1-R8 in ../ASSIGNMENT.md win over the starter code and ../roomhub.http. never change routes, json shapes or status codes. course style: RoomHub.Api/Features/<Area>/<UseCase>/ folders, Result<T> + Error(Code, Message, ErrorType), ICommand/ICommandHandler, thin minimal api endpoints; no MediatR, controllers, AutoMapper or FluentValidation. outside ./src only edit ../roomhub.http. never write ../DECISIONS.md, ../BUGS.md or ../AI-LOG.md. don't change anything yet: list every place where the starter contradicts ../ASSIGNMENT.md (rule, file, line). The format should be in md like this \|rule\|file and line\|AI\|-\|The course files that you have to use to have a better context are in X folder | tot | tot|
|5 min|do level A from ../ASSIGNMENT.md: move the code into vertical slices RoomHub.Api/Features/Rooms/{CreateRoom,GetRoom} and RoomHub.Api/Features/Bookings/{CreateBooking,GetBooking,CancelBooking}; remove the old layer folders. then dotnet build, dotnet run --project RoomHub.Api (port 5090), run every request in ../roomhub.http with curl, and add requests in section 8 of ../roomhub.http for R1-R4 (expect 400 application/problem+json) and for a missing room (expect 404 before validation). show a table: request, expected status, actual status.|||

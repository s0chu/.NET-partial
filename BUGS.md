# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| **R7**: bookings that only touch are treated as overlapping, because the check uses `<=` instead of `<`. Booking 12:00–13:00 after 10:00–12:00 returned **409**; it should be 201 | `Services/BookingService.cs:28` | AI | - |
| **R7 + R8**: a cancelled booking does not free its interval, because the overlap loop doesn't filter on `Status == Active`. Cancelling 10–12 and booking 10–12 again returned **409**; it should be 201 | `Services/BookingService.cs:26-29`, `Repositories/BookingRepository.cs:31` | AI | - |
| **R8**: cancelling an already cancelled booking returned **204**; it should be 409. The status is set without checking the current state | `Services/BookingService.cs:56-60` (line 59) | AI | - |
| **R5** (also Level D): there is no "cannot start in the past" check, and `TimeProvider` is never registered or injected. A booking starting in 2020 returned **201**; it should be 400 | `Services/BookingService.cs:6` (constructor), check missing around `:17-24`, `Program.cs:9-12` | AI | - |
| **R6**: there is no 4-hour maximum. A 4h01m booking returned **201**; it should be 400. Exactly 4h must still return 201 | `Services/BookingService.cs:17-18` (only checks end > start) | AI | - |
| **R7**: the overlap check and the insert are not atomic. `ListForRoom` and `Add` each take the lock separately, so two simultaneous requests can both pass the check. `Cancel` also changes the status outside the lock. Found by reading only | `Services/BookingService.cs:26-43`, `Repositories/BookingRepository.cs:11-33` | AI | - |
| **Contract** ("errors are ProblemDetails"): errors raised by the framework are not ProblemDetails. Malformed JSON or a missing body gives **400 `text/plain`** with a stack trace in Development (empty in Production). A non-GUID id gives **404 with an empty body** | `Program.cs:6-16` (no `AddProblemDetails()` / `UseStatusCodePages()`), `Middleware/ExceptionMiddleware.cs:13` (catches only `BusinessException`) | AI | - |
| **Contract** ("404 before any validation"): if the room doesn't exist and the body has `null` or a wrong type, model binding fails first. `"startUtc": null` returned **400** instead of 404 | `Endpoints/RoomEndpoints.cs:9` (non-nullable `DateTime`/`int`), `:33` | AI | - |
| **Contract** ("dates are in UTC"): incoming dates are not converted to UTC. `…T10:00:00+02:00` came back as `+02:00`, and a date with no offset came back without `Z`. The R3 and R7 comparisons then mix `DateTimeKind` values | `Endpoints/RoomEndpoints.cs:9`, `Services/BookingService.cs:37-38`, `Endpoints/BookingEndpoints.cs:9-10,24` | AI | - |
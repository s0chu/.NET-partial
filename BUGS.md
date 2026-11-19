# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R7: touching bookings count as overlapping (<= instead of <)| Services/BookingService.cs:28 | AI | the overlap check moved into Booking.OverlapsWith in Domain/Bookings/Booking.cs and now uses < on both sides, so bookings that only touch no longer overlap. |
| R7 + R8: a cancelled booking ignores Status | Services/BookingService.cs:26-29, Repositories/BookingRepository.cs:31 | reading |  Booking.OverlapsWith only counts Active bookings and BookingRepository.GetActiveForRoomAsync loads only Active ones, so a cancelled booking frees its interval. |
| R8: cancelling an already cancelled booking gave 204 instead of 409 (current status is never checked) | Services/BookingService.cs:56-60 (line 59) | reading | Booking.Cancel now returns BookingErrors.AlreadyCancelled (409) if the booking is already cancelled, and CancelBookingHandler returns that error. |
| R5: nothing stops bookings in the past (time provider not injected) | Services/BookingService.cs:6 , Program.cs:9-12 | AI | TimeProvider.System is registered in Program.cs, CreateBookingHandler passes GetUtcNow() into Booking.Create, and Booking.Create rejects a start before now with BookingErrors.StartInPast (400). |
| R6: no 4-hour limit | Services/BookingService.cs:17-18 (only checks end > start) | reading | Booking.Create rejects anything longer than 4 hours with BookingErrors.TooLong (400), and exactly 4 hours is allowed. |
| R7: the overlap check and insert aren't atomic| Services/BookingService.cs:26-43, Repositories/BookingRepository.cs:11-33 | AI | not fixed, because the check and the save in CreateBookingHandler are still separate steps, so two simultaneous requests can both pass; this needs a lock, a transaction or a concurrency token. |
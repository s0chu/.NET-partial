# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R7: touching bookings count as overlapping (<= instead of <)| Services/BookingService.cs:28 | AI | - |
| R7 + R8: a cancelled booking ignores Status | Services/BookingService.cs:26-29, Repositories/BookingRepository.cs:31 | reading | - |
| R8: cancelling an already cancelled booking gave 204 instead of 409 (current status is never checked) | Services/BookingService.cs:56-60 (line 59) | reading | - |
| R5: nothing stops bookings in the past (time provider not injected) | Services/BookingService.cs:6 , Program.cs:9-12 | AI | - |
| R6: no 4-hour limit | Services/BookingService.cs:17-18 (only checks end > start) | AI | - |
| R7: the overlap check and insert aren't atomic| Services/BookingService.cs:26-43, Repositories/BookingRepository.cs:11-33 | AI | - |
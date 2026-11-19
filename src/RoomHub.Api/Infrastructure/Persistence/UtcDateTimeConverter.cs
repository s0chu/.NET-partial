using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RoomHub.Api.Infrastructure.Persistence;

// SQLite stores dates as text and EF reads them back as DateTimeKind.Unspecified;
// marking them UTC keeps the "Z" in the JSON after a round trip through the database.
public sealed class UtcDateTimeConverter()
    : ValueConverter<DateTime, DateTime>(
        value => value,
        value => DateTime.SpecifyKind(value, DateTimeKind.Utc));

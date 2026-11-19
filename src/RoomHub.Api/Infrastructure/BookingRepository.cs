using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure;

// Starter: in-memory data. It is lost when the application restarts.
public class BookingRepository
{
    private readonly List<Booking> _bookings = new();
    private readonly object _lock = new();

    public void Add(Booking booking)
    {
        lock (_lock)
        {
            _bookings.Add(booking);
        }
    }

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_bookings.FirstOrDefault(b => b.Id == id));
        }
    }

    public Task<List<Booking>> ListForRoomAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_bookings.Where(b => b.RoomId == roomId).ToList());
        }
    }
}

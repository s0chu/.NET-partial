using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure;

// Starter: in-memory data. It is lost when the application restarts.
public class RoomRepository
{
    private readonly List<Room> _rooms = new();
    private readonly object _lock = new();

    public void Add(Room room)
    {
        lock (_lock)
        {
            _rooms.Add(room);
        }
    }

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_rooms.FirstOrDefault(r => r.Id == id));
        }
    }
}

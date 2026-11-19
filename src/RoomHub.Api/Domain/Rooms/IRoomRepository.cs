namespace RoomHub.Api.Domain.Rooms;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // Only marks the room as new; IUnitOfWork saves it.
    void Add(Room room);
}

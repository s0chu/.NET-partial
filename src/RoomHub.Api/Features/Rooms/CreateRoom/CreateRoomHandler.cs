using RoomHub.Api.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(RoomRepository rooms)
{
    public Room Handle(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("InvalidName", "Room name is required.", 400);

        if (capacity <= 0)
            throw new BusinessException("InvalidCapacity", "Capacity must be positive.", 400);

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Capacity = capacity
        };

        rooms.Add(room);
        return room;
    }
}

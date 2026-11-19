using RoomHub.Api.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Infrastructure;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed class GetRoomHandler(RoomRepository rooms)
{
    public Room Handle(Guid id)
    {
        var room = rooms.Get(id);
        if (room is null)
            throw new BusinessException("RoomNotFound", "The room does not exist.", 404);

        return room;
    }
}

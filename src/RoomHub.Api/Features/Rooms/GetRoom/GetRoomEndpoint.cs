using Microsoft.AspNetCore.Http.HttpResults;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed record GetRoomResponse(Guid Id, string Name, int Capacity);

public static class GetRoomEndpoint
{
    public static void Map(RouteGroupBuilder group) =>
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetRoom");

    private static Ok<GetRoomResponse> Handle(Guid id, GetRoomHandler handler)
    {
        var room = handler.Handle(id);
        return TypedResults.Ok(new GetRoomResponse(room.Id, room.Name, room.Capacity));
    }
}

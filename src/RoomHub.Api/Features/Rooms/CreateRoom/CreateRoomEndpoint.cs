using Microsoft.AspNetCore.Http.HttpResults;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomRequest(string? Name, int Capacity);

public sealed record CreateRoomResponse(Guid Id, string Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/", Handle)
            .WithName("CreateRoom");

    private static Created<CreateRoomResponse> Handle(CreateRoomRequest request, CreateRoomHandler handler)
    {
        var room = handler.Handle(request.Name, request.Capacity);
        return TypedResults.Created(
            $"/api/rooms/{room.Id}",
            new CreateRoomResponse(room.Id, room.Name, room.Capacity));
    }
}

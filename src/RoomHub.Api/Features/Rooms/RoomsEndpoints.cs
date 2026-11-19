using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;

namespace RoomHub.Api.Features.Rooms;

public static class RoomsEndpoints
{
    public static IEndpointRouteBuilder MapRooms(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/rooms").WithTags("Rooms");
        CreateRoomEndpoint.Map(group);
        GetRoomEndpoint.Map(group);
        return app;
    }
}

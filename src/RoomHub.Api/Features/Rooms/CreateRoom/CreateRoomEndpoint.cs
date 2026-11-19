using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Common;
using RoomHub.Api.Common.Cqrs;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomRequest(string? Name, int Capacity);

public sealed record CreateRoomResponse(Guid Id, string Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void Map(RouteGroupBuilder group) =>
        group.MapPost("/", Handle)
            .WithName("CreateRoom");

    private static async Task<Results<Created<CreateRoomResponse>, ProblemHttpResult>> Handle(
        CreateRoomRequest request,
        ICommandHandler<CreateRoomCommand, CreateRoomResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new CreateRoomCommand(request.Name, request.Capacity), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToProblem();

        return TypedResults.Created($"/api/rooms/{result.Value.Id}", result.Value);
    }
}

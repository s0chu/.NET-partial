using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomCommand(string? Name, int Capacity) : ICommand<CreateRoomResponse>;

public sealed class CreateRoomHandler(IRoomRepository rooms, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, CreateRoomResponse>
{
    public async Task<Result<CreateRoomResponse>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var result = Room.Create(command.Name, command.Capacity);
        if (result.IsFailure)
            return result.Error;

        var room = result.Value;
        rooms.Add(room);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateRoomResponse(room.Id, room.Name, room.Capacity);
    }
}

namespace RoomHub.Api.Domain.Common;

// The "no value" result of a command that only changes state.
public readonly record struct Unit
{
    public static readonly Unit Value = new();
}

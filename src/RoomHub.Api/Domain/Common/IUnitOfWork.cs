namespace RoomHub.Api.Domain.Common;

// Saves everything the repositories marked, in one go. Implemented by the DbContext.
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface ILogRepository : IBaseRepository<Log>
{
    Task<IEnumerable<Log>> GetByUserIdAsync(Guid userId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<Log>> GetByActionTypeAsync(string actionType, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
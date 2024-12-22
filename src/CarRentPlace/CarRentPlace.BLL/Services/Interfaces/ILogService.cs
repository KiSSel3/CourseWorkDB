using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface ILogService
{
    Task<IEnumerable<Log>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Log> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Log>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Log>> GetByActionTypeAsync(string actionType, CancellationToken cancellationToken = default);
}

using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Implementations;

public class LogService : ILogService
{
    private readonly ILogRepository _repository;

    public LogService(ILogRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Log>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(includeDeleted: false, cancellationToken);
    }

    public async Task<Log> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var log = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (log == null)
        {
            throw new KeyNotFoundException($"Log with ID '{id}' not found.");
        }

        return log;
    }

    public async Task<IEnumerable<Log>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByUserIdAsync(userId, includeDeleted: false, cancellationToken);
    }

    public async Task<IEnumerable<Log>> GetByActionTypeAsync(string actionType, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByActionTypeAsync(actionType, includeDeleted: false, cancellationToken);
    }
}
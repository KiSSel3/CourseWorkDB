using CarRentPlace.DAL.Filters;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByEmailAsync(string email, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetByFilterAsync(UserFilter filter, CancellationToken cancellationToken = default);
    Task<int> GetCountByFilterAsync(UserFilter filter, CancellationToken cancellationToken = default);
}
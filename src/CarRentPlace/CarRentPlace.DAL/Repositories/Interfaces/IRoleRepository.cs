using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default);
}

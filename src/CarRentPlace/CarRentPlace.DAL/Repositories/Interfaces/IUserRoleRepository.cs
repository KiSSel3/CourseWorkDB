using CarRentPlace.Domain.Models;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface IUserRoleRepository
{
    Task<IEnumerable<UserRole>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> GetByRoleIdAsync(Guid roleId, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
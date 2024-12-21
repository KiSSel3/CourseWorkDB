using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Models;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly IDbConnection _connection;

    public UserRoleRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<UserRole>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT user_id AS UserId, role_id AS RoleId, is_deleted AS IsDeleted
                      FROM ""UserRoles""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<UserRole>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""UserRoles"" (user_id, role_id, is_deleted)
                      VALUES (@UserId, @RoleId, @IsDeleted)";
        await _connection.ExecuteAsync(query, userRole);
    }

    public async Task DeleteAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""UserRoles""
                      WHERE user_id = @UserId AND role_id = @RoleId";
        await _connection.ExecuteAsync(query, new { UserId = userId, RoleId = roleId });
    }

    public async Task SoftDeleteAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""UserRoles""
                      SET is_deleted = TRUE
                      WHERE user_id = @UserId AND role_id = @RoleId";
        await _connection.ExecuteAsync(query, new { UserId = userId, RoleId = roleId });
    }

    public async Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT user_id AS UserId, role_id AS RoleId, is_deleted AS IsDeleted
                      FROM ""UserRoles""
                      WHERE user_id = @UserId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<UserRole>(query, new { UserId = userId, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<UserRole>> GetByRoleIdAsync(Guid roleId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT user_id AS UserId, role_id AS RoleId, is_deleted AS IsDeleted
                      FROM ""UserRoles""
                      WHERE role_id = @RoleId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<UserRole>(query, new { RoleId = roleId, IncludeDeleted = includeDeleted });
    }
}

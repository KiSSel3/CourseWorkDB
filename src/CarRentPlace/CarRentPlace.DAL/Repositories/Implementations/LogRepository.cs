using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class LogRepository : ILogRepository
{
    private readonly IDbConnection _connection;

    public LogRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Log>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, action_type AS ActionType, entity_type AS EntityType, entity_id AS EntityId, 
                             old_values AS OldValues, new_values AS NewValues, created_at AS CreatedAt, is_deleted AS IsDeleted
                      FROM ""Logs""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<Log>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<Log> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, action_type AS ActionType, entity_type AS EntityType, entity_id AS EntityId, 
                             old_values AS OldValues, new_values AS NewValues, created_at AS CreatedAt, is_deleted AS IsDeleted
                      FROM ""Logs""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<Log>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<Log>> GetByUserIdAsync(Guid userId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, action_type AS ActionType, entity_type AS EntityType, entity_id AS EntityId, 
                             old_values AS OldValues, new_values AS NewValues, created_at AS CreatedAt, is_deleted AS IsDeleted
                      FROM ""Logs""
                      WHERE user_id = @UserId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<Log>(query, new { UserId = userId, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<Log>> GetByActionTypeAsync(string actionType, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, action_type AS ActionType, entity_type AS EntityType, entity_id AS EntityId, 
                             old_values AS OldValues, new_values AS NewValues, created_at AS CreatedAt, is_deleted AS IsDeleted
                      FROM ""Logs""
                      WHERE action_type = @ActionType AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<Log>(query, new { ActionType = actionType, IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(Log entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""Logs"" (id, user_id, action_type, entity_type, entity_id, old_values, new_values, created_at, is_deleted)
                      VALUES (@Id, @UserId, @ActionType, @EntityType, @EntityId, @OldValues, @NewValues, @CreatedAt, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(Log entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Logs""
                      SET user_id = @UserId, action_type = @ActionType, entity_type = @EntityType, entity_id = @EntityId, 
                          old_values = @OldValues, new_values = @NewValues, created_at = @CreatedAt, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""Logs"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Logs"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
}
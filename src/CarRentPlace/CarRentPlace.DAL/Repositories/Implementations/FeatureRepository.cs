using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class FeatureRepository : IFeatureRepository
{
    private readonly IDbConnection _connection;

    public FeatureRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Feature>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""Features""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<Feature>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<Feature> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""Features""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<Feature>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task<Feature> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""Features""
                      WHERE name = @Name AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<Feature>(query, new { Name = name, IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(Feature entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""Features"" (id, name, is_deleted)
                      VALUES (@Id, @Name, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(Feature entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Features""
                      SET name = @Name, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""Features"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Features"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
}
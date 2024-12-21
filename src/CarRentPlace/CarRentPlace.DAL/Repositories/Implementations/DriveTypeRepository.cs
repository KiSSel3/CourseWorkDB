using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using Dapper;
using DriveType = CarRentPlace.Domain.Entities.DriveType;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class DriveTypeRepository : IDriveTypeRepository
{
    private readonly IDbConnection _connection;

    public DriveTypeRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<DriveType>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""DriveTypes""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<DriveType>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<DriveType> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""DriveTypes""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<DriveType>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task<DriveType> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""DriveTypes""
                      WHERE name = @Name AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<DriveType>(query, new { Name = name, IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(DriveType entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""DriveTypes"" (id, name, is_deleted)
                      VALUES (@Id, @Name, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(DriveType entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""DriveTypes""
                      SET name = @Name, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""DriveTypes"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""DriveTypes"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
}
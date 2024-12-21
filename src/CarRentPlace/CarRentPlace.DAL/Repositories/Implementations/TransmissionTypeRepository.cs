using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class TransmissionTypeRepository : ITransmissionTypeRepository
{
    private readonly IDbConnection _connection;

    public TransmissionTypeRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<TransmissionType>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""TransmissionTypes""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<TransmissionType>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<TransmissionType> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""TransmissionTypes""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<TransmissionType>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task<TransmissionType> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""TransmissionTypes""
                      WHERE name = @Name AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<TransmissionType>(query, new { Name = name, IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(TransmissionType entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""TransmissionTypes"" (id, name, is_deleted)
                      VALUES (@Id, @Name, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(TransmissionType entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""TransmissionTypes""
                      SET name = @Name, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""TransmissionTypes"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""TransmissionTypes"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
}

using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class CarModelRepository : ICarModelRepository
{
    private readonly IDbConnection _connection;

    public CarModelRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<CarModel>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""CarModels""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<CarModel>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<CarModel> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""CarModels""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<CarModel>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task<CarModel> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""CarModels""
                      WHERE name = @Name AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<CarModel>(query, new { Name = name, IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(CarModel entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""CarModels"" (id, name, is_deleted)
                      VALUES (@Id, @Name, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(CarModel entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""CarModels""
                      SET name = @Name, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""CarModels"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""CarModels"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
}
using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class CarBodyTypeRepository : ICarBodyTypeRepository
{
    private readonly IDbConnection _connection;

    public CarBodyTypeRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<CarBodyType> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""CarBodyTypes""
                      WHERE name = @Name AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<CarBodyType>(query, new { Name = name, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<CarBodyType>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""CarBodyTypes""
                      WHERE (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<CarBodyType>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(CarBodyType entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""CarBodyTypes"" (name, is_deleted)
                      VALUES (@Name, @IsDeleted)";
        await _connection.ExecuteAsync(query, new { Name = entity.Name, IsDeleted = entity.IsDeleted });
    }

    public async Task UpdateAsync(CarBodyType entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""CarBodyTypes""
                      SET name = @Name, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = entity.Id, Name = entity.Name, IsDeleted = entity.IsDeleted });
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""CarBodyTypes"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""CarBodyTypes"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task<CarBodyType> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, is_deleted AS IsDeleted
                      FROM ""CarBodyTypes""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<CarBodyType>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }
}

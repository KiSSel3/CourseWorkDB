using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class CarBrandRepository : ICarBrandRepository
{
    private readonly IDbConnection _connection;

    public CarBrandRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<CarBrand>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, country AS Country, is_deleted AS IsDeleted
                      FROM ""CarBrands""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<CarBrand>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<CarBrand> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, country AS Country, is_deleted AS IsDeleted
                      FROM ""CarBrands""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<CarBrand>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task<CarBrand> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, name AS Name, country AS Country, is_deleted AS IsDeleted
                      FROM ""CarBrands""
                      WHERE name = @Name AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<CarBrand>(query, new { Name = name, IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(CarBrand entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""CarBrands"" (id, name, country, is_deleted)
                      VALUES (@Id, @Name, @Country, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(CarBrand entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""CarBrands""
                      SET name = @Name, country = @Country, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""CarBrands"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""CarBrands"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
}

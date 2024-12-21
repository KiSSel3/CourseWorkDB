using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Models;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class CarFeatureRepository : ICarFeatureRepository
{
    private readonly IDbConnection _connection;

    public CarFeatureRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<CarFeature>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT car_id AS CarId, feature_id AS FeatureId, is_deleted AS IsDeleted
                      FROM ""CarFeatures""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<CarFeature>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<CarFeature>> GetByCarIdAsync(Guid carId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT car_id AS CarId, feature_id AS FeatureId, is_deleted AS IsDeleted
                      FROM ""CarFeatures""
                      WHERE car_id = @CarId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<CarFeature>(query, new { CarId = carId, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<CarFeature>> GetByFeatureIdAsync(Guid featureId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT car_id AS CarId, feature_id AS FeatureId, is_deleted AS IsDeleted
                      FROM ""CarFeatures""
                      WHERE feature_id = @FeatureId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<CarFeature>(query, new { FeatureId = featureId, IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(CarFeature carFeature, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""CarFeatures"" (car_id, feature_id, is_deleted)
                      VALUES (@CarId, @FeatureId, @IsDeleted)";
        await _connection.ExecuteAsync(query, carFeature);
    }

    public async Task DeleteAsync(Guid carId, Guid featureId, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""CarFeatures""
                      WHERE car_id = @CarId AND feature_id = @FeatureId";
        await _connection.ExecuteAsync(query, new { CarId = carId, FeatureId = featureId });
    }

    public async Task SoftDeleteAsync(Guid carId, Guid featureId, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""CarFeatures""
                      SET is_deleted = TRUE
                      WHERE car_id = @CarId AND feature_id = @FeatureId";
        await _connection.ExecuteAsync(query, new { CarId = carId, FeatureId = featureId });
    }
}
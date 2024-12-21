using System.Data;
using CarRentPlace.DAL.Filters;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class CarRepository : ICarRepository
{
    private readonly IDbConnection _connection;

    public CarRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Car>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, brand_id AS BrandId, model_id AS ModelId, body_type_id AS BodyTypeId, 
                             transmission_type_id AS TransmissionTypeId, drive_type_id AS DriveTypeId, seats AS Seats, 
                             mileage AS Mileage, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Cars""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<Car>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<Car> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, brand_id AS BrandId, model_id AS ModelId, body_type_id AS BodyTypeId, 
                             transmission_type_id AS TransmissionTypeId, drive_type_id AS DriveTypeId, seats AS Seats, 
                             mileage AS Mileage, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Cars""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<Car>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<Car>> GetByBrandIdAsync(Guid brandId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, brand_id AS BrandId, model_id AS ModelId, body_type_id AS BodyTypeId, 
                             transmission_type_id AS TransmissionTypeId, drive_type_id AS DriveTypeId, seats AS Seats, 
                             mileage AS Mileage, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Cars""
                      WHERE brand_id = @BrandId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<Car>(query, new { BrandId = brandId, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<Car>> GetByModelIdAsync(Guid modelId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, brand_id AS BrandId, model_id AS ModelId, body_type_id AS BodyTypeId, 
                             transmission_type_id AS TransmissionTypeId, drive_type_id AS DriveTypeId, seats AS Seats, 
                             mileage AS Mileage, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Cars""
                      WHERE model_id = @ModelId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<Car>(query, new { ModelId = modelId, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<Car>> GetByFilterAsync(CarFilter filter, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, brand_id AS BrandId, model_id AS ModelId, body_type_id AS BodyTypeId, 
                             transmission_type_id AS TransmissionTypeId, drive_type_id AS DriveTypeId, seats AS Seats, 
                             mileage AS Mileage, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Cars""
                      WHERE (@BrandId IS NULL OR brand_id = @BrandId)
                        AND (@ModelId IS NULL OR model_id = @ModelId)
                        AND (@BodyTypeId IS NULL OR body_type_id = @BodyTypeId)
                        AND (@TransmissionTypeId IS NULL OR transmission_type_id = @TransmissionTypeId)
                        AND (@DriveTypeId IS NULL OR drive_type_id = @DriveTypeId)
                        AND (@MinSeats IS NULL OR seats >= @MinSeats)
                        AND (@MaxSeats IS NULL OR seats <= @MaxSeats)
                        AND (@MinMileage IS NULL OR mileage >= @MinMileage)
                        AND (@MaxMileage IS NULL OR mileage <= @MaxMileage)
                        AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)
                      LIMIT @PageSize OFFSET @Offset";

        var parameters = new
        {
            filter.BrandId,
            filter.ModelId,
            filter.BodyTypeId,
            filter.TransmissionTypeId,
            filter.DriveTypeId,
            filter.MinSeats,
            filter.MaxSeats,
            filter.MinMileage,
            filter.MaxMileage,
            filter.IncludeDeleted,
            PageSize = filter.PageSize,
            Offset = (filter.PageNumber - 1) * filter.PageSize
        };

        return await _connection.QueryAsync<Car>(query, parameters);
    }

    public async Task<int> GetCountByFilterAsync(CarFilter filter, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT COUNT(*)
                      FROM ""Cars""
                      WHERE (@BrandId IS NULL OR brand_id = @BrandId)
                        AND (@ModelId IS NULL OR model_id = @ModelId)
                        AND (@BodyTypeId IS NULL OR body_type_id = @BodyTypeId)
                        AND (@TransmissionTypeId IS NULL OR transmission_type_id = @TransmissionTypeId)
                        AND (@DriveTypeId IS NULL OR drive_type_id = @DriveTypeId)
                        AND (@MinSeats IS NULL OR seats >= @MinSeats)
                        AND (@MaxSeats IS NULL OR seats <= @MaxSeats)
                        AND (@MinMileage IS NULL OR mileage >= @MinMileage)
                        AND (@MaxMileage IS NULL OR mileage <= @MaxMileage)
                        AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";

        var parameters = new
        {
            filter.BrandId,
            filter.ModelId,
            filter.BodyTypeId,
            filter.TransmissionTypeId,
            filter.DriveTypeId,
            filter.MinSeats,
            filter.MaxSeats,
            filter.MinMileage,
            filter.MaxMileage,
            filter.IncludeDeleted
        };

        return await _connection.ExecuteScalarAsync<int>(query, parameters);
    }

    public async Task AddAsync(Car entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""Cars"" (id, brand_id, model_id, body_type_id, transmission_type_id, drive_type_id, seats, mileage, created_at, updated_at, is_deleted)
                      VALUES (@Id, @BrandId, @ModelId, @BodyTypeId, @TransmissionTypeId, @DriveTypeId, @Seats, @Mileage, @CreatedAt, @UpdatedAt, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(Car entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Cars""
                      SET brand_id = @BrandId, model_id = @ModelId, body_type_id = @BodyTypeId, transmission_type_id = @TransmissionTypeId, 
                          drive_type_id = @DriveTypeId, seats = @Seats, mileage = @Mileage, created_at = @CreatedAt, updated_at = @UpdatedAt, 
                          is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""Cars"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Cars"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
}
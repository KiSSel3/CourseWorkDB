using System.Data;
using CarRentPlace.DAL.Filters;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class RentOfferRepository : IRentOfferRepository
{
    private readonly IDbConnection _connection;

    public RentOfferRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<RentOffer>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, car_id AS CarId, price_per_day AS PricePerDay, location AS Location, 
                             created_at AS CreatedAt, updated_at AS UpdatedAt, description AS Description, is_available AS IsAvailable, 
                             is_deleted AS IsDeleted
                      FROM ""RentOffers""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<RentOffer>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<RentOffer> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, car_id AS CarId, price_per_day AS PricePerDay, location AS Location, 
                             created_at AS CreatedAt, updated_at AS UpdatedAt, description AS Description, is_available AS IsAvailable, 
                             is_deleted AS IsDeleted
                      FROM ""RentOffers""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<RentOffer>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(RentOffer entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""RentOffers"" (id, user_id, car_id, price_per_day, location, created_at, updated_at, description, is_available, is_deleted)
                      VALUES (@Id, @UserId, @CarId, @PricePerDay, @Location, @CreatedAt, @UpdatedAt, @Description, @IsAvailable, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(RentOffer entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""RentOffers""
                      SET user_id = @UserId, car_id = @CarId, price_per_day = @PricePerDay, location = @Location, 
                          created_at = @CreatedAt, updated_at = @UpdatedAt, description = @Description, 
                          is_available = @IsAvailable, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""RentOffers"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""RentOffers"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task<IEnumerable<RentOffer>> GetAvailableOffersAsync(CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, car_id AS CarId, price_per_day AS PricePerDay, location AS Location, 
                             created_at AS CreatedAt, updated_at AS UpdatedAt, description AS Description, is_available AS IsAvailable, 
                             is_deleted AS IsDeleted
                      FROM ""RentOffers""
                      WHERE is_available = TRUE AND is_deleted = FALSE";
        return await _connection.QueryAsync<RentOffer>(query);
    }

    public async Task<IEnumerable<RentOffer>> GetByUserIdAsync(Guid userId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, car_id AS CarId, price_per_day AS PricePerDay, location AS Location, 
                             created_at AS CreatedAt, updated_at AS UpdatedAt, description AS Description, is_available AS IsAvailable, 
                             is_deleted AS IsDeleted
                      FROM ""RentOffers""
                      WHERE user_id = @UserId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<RentOffer>(query, new { UserId = userId, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<RentOffer>> GetByFilterAsync(RentOfferFilter filter, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, car_id AS CarId, price_per_day AS PricePerDay, location AS Location, 
                             created_at AS CreatedAt, updated_at AS UpdatedAt, description AS Description, is_available AS IsAvailable, 
                             is_deleted AS IsDeleted
                      FROM ""RentOffers""
                      WHERE (@UserId IS NULL OR user_id = @UserId)
                        AND (@CarId IS NULL OR car_id = @CarId)
                        AND (@MinPricePerDay IS NULL OR price_per_day >= @MinPricePerDay)
                        AND (@MaxPricePerDay IS NULL OR price_per_day <= @MaxPricePerDay)
                        AND (@Location IS NULL OR location ILIKE '%' || @Location || '%')
                        AND (is_available = @IsAvailable)
                        AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)
                      LIMIT @PageSize OFFSET @Offset";

        var parameters = new
        {
            filter.UserId,
            filter.CarId,
            filter.MinPricePerDay,
            filter.MaxPricePerDay,
            filter.Location,
            filter.IsAvailable,
            filter.IncludeDeleted,
            PageSize = filter.PageSize,
            Offset = (filter.PageNumber - 1) * filter.PageSize
        };

        return await _connection.QueryAsync<RentOffer>(query, parameters);
    }

    public async Task<int> GetCountByFilterAsync(RentOfferFilter filter, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT COUNT(*)
                      FROM ""RentOffers""
                      WHERE (@UserId IS NULL OR user_id = @UserId)
                        AND (@CarId IS NULL OR car_id = @CarId)
                        AND (@MinPricePerDay IS NULL OR price_per_day >= @MinPricePerDay)
                        AND (@MaxPricePerDay IS NULL OR price_per_day <= @MaxPricePerDay)
                        AND (@Location IS NULL OR location ILIKE '%' || @Location || '%')
                        AND (is_available = @IsAvailable)
                        AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";

        var parameters = new
        {
            filter.UserId,
            filter.CarId,
            filter.MinPricePerDay,
            filter.MaxPricePerDay,
            filter.Location,
            filter.IsAvailable,
            filter.IncludeDeleted
        };

        return await _connection.ExecuteScalarAsync<int>(query, parameters);
    }
}
using System.Data;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class RentOfferImageRepository : IRentOfferImageRepository
{
    private readonly IDbConnection _connection;

    public RentOfferImageRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<RentOfferImage>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, rent_offer_id AS RentOfferId, image_url AS ImageUrl, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""RentOfferImages""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<RentOfferImage>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<RentOfferImage> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, rent_offer_id AS RentOfferId, image_url AS ImageUrl, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""RentOfferImages""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<RentOfferImage>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<RentOfferImage>> GetByRentOfferIdAsync(Guid rentOfferId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, rent_offer_id AS RentOfferId, image_url AS ImageUrl, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""RentOfferImages""
                      WHERE rent_offer_id = @RentOfferId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<RentOfferImage>(query, new { RentOfferId = rentOfferId, IncludeDeleted = includeDeleted });
    }

    public async Task AddAsync(RentOfferImage entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""RentOfferImages"" (id, rent_offer_id, image_url, created_at, updated_at, is_deleted)
                      VALUES (@Id, @RentOfferId, @ImageUrl, @CreatedAt, @UpdatedAt, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(RentOfferImage entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""RentOfferImages""
                      SET rent_offer_id = @RentOfferId, image_url = @ImageUrl, created_at = @CreatedAt, updated_at = @UpdatedAt, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""RentOfferImages"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""RentOfferImages"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
}

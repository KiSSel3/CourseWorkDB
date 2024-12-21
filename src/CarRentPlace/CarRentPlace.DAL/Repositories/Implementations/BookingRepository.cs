using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CarRentPlace.DAL.Filters;
using CarRentPlace.DAL.Repositories.Implementations;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using CarRentPlace.Domain.Enums;
using Dapper;

//TODO: протестировать
public class BookingRepository : IBookingRepository
{
    private readonly IDbConnection _connection;

    public BookingRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, rent_offer_id AS RentOfferId, start_date AS StartDate, end_date AS EndDate, 
                             total_price AS TotalPrice, status AS Status, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Bookings""
                      WHERE user_id = @UserId AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<Booking>(query, new { UserId = userId, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, rent_offer_id AS RentOfferId, start_date AS StartDate, end_date AS EndDate, 
                             total_price AS TotalPrice, status AS Status, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Bookings""
                      WHERE status = @Status AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QueryAsync<Booking>(query, new { Status = status, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<Booking>> GetByFilterAsync(BookingFilter filter, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, rent_offer_id AS RentOfferId, start_date AS StartDate, end_date AS EndDate, 
                             total_price AS TotalPrice, status AS Status, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Bookings""
                      WHERE (@UserId IS NULL OR user_id = @UserId)
                        AND (@RentOfferId IS NULL OR rent_offer_id = @RentOfferId)
                        AND (@Status IS NULL OR status = @Status)
                        AND (@StartDate IS NULL OR start_date >= @StartDate)
                        AND (@EndDate IS NULL OR end_date <= @EndDate)
                        AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)
                      LIMIT @PageSize OFFSET @Offset";

        var parameters = new
        {
            UserId = filter.UserId,
            RentOfferId = filter.RentOfferId,
            Status = filter.Status,
            StartDate = filter.StartDate,
            EndDate = filter.EndDate,
            IncludeDeleted = filter.IncludeDeleted,
            PageSize = filter.PageSize,
            Offset = (filter.PageNumber - 1) * filter.PageSize
        };

        return await _connection.QueryAsync<Booking>(query, parameters);
    }

    public async Task<int> GetCountByFilterAsync(BookingFilter filter, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT COUNT(*)
                      FROM ""Bookings""
                      WHERE (@UserId IS NULL OR user_id = @UserId)
                        AND (@RentOfferId IS NULL OR rent_offer_id = @RentOfferId)
                        AND (@Status IS NULL OR status = @Status)
                        AND (@StartDate IS NULL OR start_date >= @StartDate)
                        AND (@EndDate IS NULL OR end_date <= @EndDate)
                        AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";

        var parameters = new
        {
            UserId = filter.UserId,
            RentOfferId = filter.RentOfferId,
            Status = filter.Status,
            StartDate = filter.StartDate,
            EndDate = filter.EndDate,
            IncludeDeleted = filter.IncludeDeleted
        };

        return await _connection.ExecuteScalarAsync<int>(query, parameters);
    }

    public async Task AddAsync(Booking entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""Bookings"" (user_id, rent_offer_id, start_date, end_date, total_price, status, created_at, updated_at, is_deleted)
                      VALUES (@UserId, @RentOfferId, @StartDate, @EndDate, @TotalPrice, @Status, @CreatedAt, @UpdatedAt, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(Booking entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Bookings""
                      SET user_id = @UserId, rent_offer_id = @RentOfferId, start_date = @StartDate, end_date = @EndDate, 
                          total_price = @TotalPrice, status = @Status, created_at = @CreatedAt, updated_at = @UpdatedAt, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""Bookings"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Bookings"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
    
    public async Task<IEnumerable<Booking>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, rent_offer_id AS RentOfferId, start_date AS StartDate, end_date AS EndDate, 
                             total_price AS TotalPrice, status AS Status, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Bookings""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<Booking>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<Booking> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, user_id AS UserId, rent_offer_id AS RentOfferId, start_date AS StartDate, end_date AS EndDate, 
                             total_price AS TotalPrice, status AS Status, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Bookings""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<Booking>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }
}


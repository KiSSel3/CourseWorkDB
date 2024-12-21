using System.Data;
using CarRentPlace.DAL.Filters;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using Dapper;

namespace CarRentPlace.DAL.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;

    public UserRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<User>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, first_name AS FirstName, last_name AS LastName, email AS Email, phone_number AS PhoneNumber, 
                             password_hash AS PasswordHash, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Users""
                      WHERE is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE";
        return await _connection.QueryAsync<User>(query, new { IncludeDeleted = includeDeleted });
    }

    public async Task<User> GetByIdAsync(Guid id, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, first_name AS FirstName, last_name AS LastName, email AS Email, phone_number AS PhoneNumber, 
                             password_hash AS PasswordHash, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Users""
                      WHERE id = @Id AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<User>(query, new { Id = id, IncludeDeleted = includeDeleted });
    }

    public async Task<User> GetByEmailAsync(string email, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, first_name AS FirstName, last_name AS LastName, email AS Email, phone_number AS PhoneNumber, 
                             password_hash AS PasswordHash, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Users""
                      WHERE email = @Email AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";
        return await _connection.QuerySingleOrDefaultAsync<User>(query, new { Email = email, IncludeDeleted = includeDeleted });
    }

    public async Task<IEnumerable<User>> GetByFilterAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT id AS Id, first_name AS FirstName, last_name AS LastName, email AS Email, phone_number AS PhoneNumber, 
                             password_hash AS PasswordHash, created_at AS CreatedAt, updated_at AS UpdatedAt, is_deleted AS IsDeleted
                      FROM ""Users""
                      WHERE (@FirstName IS NULL OR first_name ILIKE '%' || @FirstName || '%')
                        AND (@LastName IS NULL OR last_name ILIKE '%' || @LastName || '%')
                        AND (@Email IS NULL OR email ILIKE '%' || @Email || '%')
                        AND (@PhoneNumber IS NULL OR phone_number ILIKE '%' || @PhoneNumber || '%')
                        AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";

        var parameters = new
        {
            filter.FirstName,
            filter.LastName,
            filter.Email,
            filter.PhoneNumber,
            filter.IncludeDeleted,
        };

        return await _connection.QueryAsync<User>(query, parameters);
    }

    public async Task<int> GetCountByFilterAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var query = @"SELECT COUNT(*)
                      FROM ""Users""
                      WHERE (@FirstName IS NULL OR first_name ILIKE '%' || @FirstName || '%')
                        AND (@LastName IS NULL OR last_name ILIKE '%' || @LastName || '%')
                        AND (@Email IS NULL OR email ILIKE '%' || @Email || '%')
                        AND (@PhoneNumber IS NULL OR phone_number ILIKE '%' || @PhoneNumber || '%')
                        AND (is_deleted = @IncludeDeleted OR @IncludeDeleted = TRUE)";

        var parameters = new
        {
            filter.FirstName,
            filter.LastName,
            filter.Email,
            filter.PhoneNumber,
            filter.IncludeDeleted
        };

        return await _connection.ExecuteScalarAsync<int>(query, parameters);
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default)
    {
        var query = @"INSERT INTO ""Users"" (id, first_name, last_name, email, phone_number, password_hash, created_at, updated_at, is_deleted)
                      VALUES (@Id, @FirstName, @LastName, @Email, @PhoneNumber, @PasswordHash, @CreatedAt, @UpdatedAt, @IsDeleted)";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Users""
                      SET first_name = @FirstName, last_name = @LastName, email = @Email, phone_number = @PhoneNumber, 
                          password_hash = @PasswordHash, created_at = @CreatedAt, updated_at = @UpdatedAt, is_deleted = @IsDeleted
                      WHERE id = @Id";
        await _connection.ExecuteAsync(query, entity);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"DELETE FROM ""Users"" WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = @"UPDATE ""Users"" SET is_deleted = TRUE WHERE id = @Id";
        await _connection.ExecuteAsync(query, new { Id = id });
    }
}
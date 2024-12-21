using CarRentPlace.DAL.Filters;
using CarRentPlace.Domain.Entities;
using CarRentPlace.Domain.Enums;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface IBookingRepository : IBaseRepository<Booking>
{
    Task<IEnumerable<Booking>> GetByUserIdAsync(Guid userId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByStatusAsync(BookingStatus status, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByFilterAsync(BookingFilter filter, CancellationToken cancellationToken = default);
    Task<int> GetCountByFilterAsync(BookingFilter filter, CancellationToken cancellationToken = default);
}
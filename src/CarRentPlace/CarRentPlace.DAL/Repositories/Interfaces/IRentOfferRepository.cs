using CarRentPlace.DAL.Filters;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface IRentOfferRepository : IBaseRepository<RentOffer>
{
    Task<IEnumerable<RentOffer>> GetAvailableOffersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RentOffer>> GetByUserIdAsync(Guid userId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<RentOffer>> GetByFilterAsync(RentOfferFilter filter, CancellationToken cancellationToken = default);
    Task<int> GetCountByFilterAsync(RentOfferFilter filter, CancellationToken cancellationToken = default);
}
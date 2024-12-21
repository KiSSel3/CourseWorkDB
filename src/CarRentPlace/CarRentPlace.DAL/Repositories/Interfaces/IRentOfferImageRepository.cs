using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface IRentOfferImageRepository : IBaseRepository<RentOfferImage>
{
    Task<IEnumerable<RentOfferImage>> GetByRentOfferIdAsync(Guid rentOfferId, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface IRentOfferImageService
{
    Task<IEnumerable<RentOfferImage>> GetByRentOfferIdAsync(Guid rentOfferId, CancellationToken cancellationToken = default);
    Task CreateAsync(Guid rentOfferId, string imageUrl, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

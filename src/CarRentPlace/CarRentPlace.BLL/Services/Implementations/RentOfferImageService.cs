using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Implementations;

public class RentOfferImageService : IRentOfferImageService
{
    private readonly IRentOfferImageRepository _repository;

    public RentOfferImageService(IRentOfferImageRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RentOfferImage>> GetByRentOfferIdAsync(Guid rentOfferId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByRentOfferIdAsync(rentOfferId, includeDeleted: false, cancellationToken);
    }

    public async Task CreateAsync(Guid rentOfferId, string imageUrl, CancellationToken cancellationToken = default)
    {
        var newImage = new RentOfferImage
        {
            Id = Guid.NewGuid(),
            RentOfferId = rentOfferId,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(newImage, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rentOfferImage = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (rentOfferImage == null)
        {
            throw new KeyNotFoundException($"Rent offer image with ID '{id}' not found.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
    }
}

using CarRentPlace.BLL.Helpers;
using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.BLL.ViewModels.RentOffer;
using CarRentPlace.DAL.Filters;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Implementations;

public class RentOfferService : IRentOfferService
{
    private readonly IRentOfferRepository _repository;

    public RentOfferService(IRentOfferRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedList<RentOffer>> GetByFilterAsync(RentOfferFilter filter, CancellationToken cancellationToken = default)
    {
        var offers = await _repository.GetByFilterAsync(filter, cancellationToken);
        var totalCount = await _repository.GetCountByFilterAsync(filter, cancellationToken);

        return new PagedList<RentOffer>(
            offers.ToList(),
            totalCount,
            filter.PageNumber,
            filter.PageSize
        );
    }

    public async Task<RentOffer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rentOffer = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (rentOffer == null)
        {
            throw new KeyNotFoundException($"Rent offer with ID '{id}' not found.");
        }

        return rentOffer;
    }

    public async Task<IEnumerable<RentOffer>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetByUserIdAsync(userId, includeDeleted: false, cancellationToken);
    }

    public async Task CreateAsync(CreateRentOfferViewModel model, CancellationToken cancellationToken = default)
    {
        var newRentOffer = new RentOffer
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            CarId = model.CarId,
            PricePerDay = model.PricePerDay,
            Location = model.Location,
            Description = model.Description,
            IsAvailable = model.IsAvailable,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(newRentOffer, cancellationToken);
    }

    public async Task UpdateAsync(UpdateRentOfferViewModel model, CancellationToken cancellationToken = default)
    {
        var existingOffer = await _repository.GetByIdAsync(model.Id, includeDeleted: false, cancellationToken);
        if (existingOffer == null)
        {
            throw new KeyNotFoundException($"Rent offer with ID '{model.Id}' not found.");
        }

        existingOffer.CarId = model.CarId;
        existingOffer.PricePerDay = model.PricePerDay;
        existingOffer.Location = model.Location;
        existingOffer.Description = model.Description;
        existingOffer.IsAvailable = model.IsAvailable;
        existingOffer.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existingOffer, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var rentOffer = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (rentOffer == null)
        {
            throw new KeyNotFoundException($"Rent offer with ID '{id}' not found.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
    }
}
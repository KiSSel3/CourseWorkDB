using CarRentPlace.BLL.Helpers;
using CarRentPlace.BLL.ViewModels.RentOffer;
using CarRentPlace.DAL.Filters;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface IRentOfferService
{
    Task<PagedList<RentOffer>> GetByFilterAsync(RentOfferFilter filter, CancellationToken cancellationToken = default);
    Task<RentOffer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<RentOffer>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task CreateAsync(CreateRentOfferViewModel model, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateRentOfferViewModel model, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

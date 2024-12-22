using CarRentPlace.BLL.Helpers;
using CarRentPlace.BLL.ViewModels.Car;
using CarRentPlace.DAL.Filters;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface ICarService
{
    Task<PagedList<Car>> GetByFilterAsync(CarFilter filter, CancellationToken cancellationToken = default);
    Task<Car> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(CreateCarViewModel model, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateCarViewModel model, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddFeatureToCarAsync(Guid carId, string featureName, CancellationToken cancellationToken = default);
    Task RemoveFeatureFromCarAsync(Guid carId, string featureName, CancellationToken cancellationToken = default);
}

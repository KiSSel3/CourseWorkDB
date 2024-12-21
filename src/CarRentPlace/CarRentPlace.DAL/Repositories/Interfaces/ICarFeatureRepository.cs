using CarRentPlace.Domain.Models;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface ICarFeatureRepository
{
    Task<IEnumerable<CarFeature>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task AddAsync(CarFeature carFeature, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid carId, Guid featureId, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid carId, Guid featureId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CarFeature>> GetByCarIdAsync(Guid carId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<CarFeature>> GetByFeatureIdAsync(Guid featureId, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
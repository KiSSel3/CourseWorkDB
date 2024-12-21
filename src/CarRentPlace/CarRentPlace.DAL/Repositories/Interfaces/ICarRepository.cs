using CarRentPlace.DAL.Filters;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface ICarRepository : IBaseRepository<Car>
{
    Task<IEnumerable<Car>> GetByBrandIdAsync(Guid brandId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<Car>> GetByModelIdAsync(Guid modelId, bool includeDeleted = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<Car>> GetByFilterAsync(CarFilter filter, CancellationToken cancellationToken = default);
    Task<int> GetCountByFilterAsync(CarFilter filter, CancellationToken cancellationToken = default);
}
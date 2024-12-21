using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface ICarBrandRepository : IBaseRepository<CarBrand>
{
    Task<CarBrand> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
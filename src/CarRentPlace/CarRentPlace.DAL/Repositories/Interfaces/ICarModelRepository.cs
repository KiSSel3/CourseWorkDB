using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface ICarModelRepository : IBaseRepository<CarModel>
{
    Task<CarModel> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface ICarBodyTypeRepository : IBaseRepository<CarBodyType>
{
    Task<CarBodyType> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
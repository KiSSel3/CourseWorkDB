using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface IFeatureRepository : IBaseRepository<Feature>
{
    Task<Feature> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
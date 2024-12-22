using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface IFeatureService
{
    Task<IEnumerable<Feature>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Feature> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Feature> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task CreateAsync(string name, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface ICarModelService
{
    Task<IEnumerable<CarModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CarModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(string name, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

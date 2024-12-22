using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface ICarBodyTypeService
{
    Task<IEnumerable<CarBodyType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CarBodyType> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(string name, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

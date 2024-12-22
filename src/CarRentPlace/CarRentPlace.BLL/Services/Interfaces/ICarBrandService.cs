using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface ICarBrandService
{
    Task<IEnumerable<CarBrand>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CarBrand> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(string name, string country, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

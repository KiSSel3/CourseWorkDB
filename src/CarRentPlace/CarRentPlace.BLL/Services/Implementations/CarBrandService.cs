using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Implementations;

public class CarBrandService : ICarBrandService
{
    private readonly ICarBrandRepository _repository;

    public CarBrandService(ICarBrandRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CarBrand>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(includeDeleted: false, cancellationToken);
    }

    public async Task<CarBrand> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var carBrand = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (carBrand == null)
        {
            throw new KeyNotFoundException($"Car brand with ID '{id}' not found.");
        }

        return carBrand;
    }

    public async Task CreateAsync(string name, string country, CancellationToken cancellationToken = default)
    {
        var existingBrand = await _repository.GetByNameAsync(name, includeDeleted: true, cancellationToken);
        if (existingBrand != null)
        {
            throw new InvalidOperationException($"A car brand with the name '{name}' already exists.");
        }

        var newCarBrand = new CarBrand
        {
            Id = Guid.NewGuid(),
            Name = name,
            Country = country
        };

        await _repository.AddAsync(newCarBrand, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var carBrand = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (carBrand == null)
        {
            throw new KeyNotFoundException($"Car brand with ID '{id}' not found.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
    }
}

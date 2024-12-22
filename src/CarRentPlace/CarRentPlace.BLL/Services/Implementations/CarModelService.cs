using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Implementations;

public class CarModelService : ICarModelService
{
    private readonly ICarModelRepository _repository;

    public CarModelService(ICarModelRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CarModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(includeDeleted: false, cancellationToken);
    }

    public async Task<CarModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var carModel = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (carModel == null)
        {
            throw new KeyNotFoundException($"Car model with ID '{id}' not found.");
        }

        return carModel;
    }

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var existingModel = await _repository.GetByNameAsync(name, includeDeleted: true, cancellationToken);
        if (existingModel != null)
        {
            throw new InvalidOperationException($"A car model with the name '{name}' already exists.");
        }

        var newCarModel = new CarModel
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        await _repository.AddAsync(newCarModel, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var carModel = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (carModel == null)
        {
            throw new KeyNotFoundException($"Car model with ID '{id}' not found.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
    }
}
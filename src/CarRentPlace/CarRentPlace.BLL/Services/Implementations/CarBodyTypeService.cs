using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Implementations;

public class CarBodyTypeService : ICarBodyTypeService
{
    private readonly ICarBodyTypeRepository _repository;

    public CarBodyTypeService(ICarBodyTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CarBodyType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(includeDeleted: false, cancellationToken);
    }

    public async Task<CarBodyType> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var carBodyType = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (carBodyType == null)
        {
            throw new KeyNotFoundException($"Car body type with ID '{id}' not found.");
        }

        return carBodyType;
    }

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByNameAsync(name, includeDeleted: true, cancellationToken);
        if (existing != null)
        {
            throw new InvalidOperationException($"A car body type with the name '{name}' already exists.");
        }

        var newCarBodyType = new CarBodyType
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        await _repository.AddAsync(newCarBodyType, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var carBodyType = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (carBodyType == null)
        {
            throw new KeyNotFoundException($"Car body type with ID '{id}' not found.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
    }
}

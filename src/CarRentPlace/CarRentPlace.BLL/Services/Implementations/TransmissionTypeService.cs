using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Implementations;

public class TransmissionTypeService : ITransmissionTypeService
{
    private readonly ITransmissionTypeRepository _repository;

    public TransmissionTypeService(ITransmissionTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TransmissionType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(includeDeleted: false, cancellationToken);
    }

    public async Task<TransmissionType> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transmissionType = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (transmissionType == null)
        {
            throw new KeyNotFoundException($"Transmission type with ID '{id}' not found.");
        }

        return transmissionType;
    }
    
    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var existingType = await _repository.GetByNameAsync(name, includeDeleted: true, cancellationToken);
        if (existingType != null)
        {
            throw new InvalidOperationException($"A transmission type with the name '{name}' already exists.");
        }

        var newTransmissionType = new TransmissionType
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        await _repository.AddAsync(newTransmissionType, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var transmissionType = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (transmissionType == null)
        {
            throw new KeyNotFoundException($"Transmission type with ID '{id}' not found.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
    }
}

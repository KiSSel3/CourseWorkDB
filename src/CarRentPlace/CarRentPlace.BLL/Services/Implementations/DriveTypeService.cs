using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Interfaces;
using DriveType = CarRentPlace.Domain.Entities.DriveType;

namespace CarRentPlace.BLL.Services.Implementations;

public class DriveTypeService : IDriveTypeService
{
    private readonly IDriveTypeRepository _repository;

    public DriveTypeService(IDriveTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DriveType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(includeDeleted: false, cancellationToken);
    }

    public async Task<DriveType> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var driveType = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (driveType == null)
        {
            throw new KeyNotFoundException($"Drive type with ID '{id}' not found.");
        }

        return driveType;
    }

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var existingDriveType = await _repository.GetByNameAsync(name, includeDeleted: true, cancellationToken);
        if (existingDriveType != null)
        {
            throw new InvalidOperationException($"A drive type with the name '{name}' already exists.");
        }

        var newDriveType = new DriveType
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        await _repository.AddAsync(newDriveType, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var driveType = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (driveType == null)
        {
            throw new KeyNotFoundException($"Drive type with ID '{id}' not found.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
    }
}

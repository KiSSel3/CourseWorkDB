using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Implementations;

public class FeatureService : IFeatureService
{
    private readonly IFeatureRepository _repository;

    public FeatureService(IFeatureRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Feature>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(includeDeleted: false, cancellationToken);
    }

    public async Task<Feature> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var feature = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (feature == null)
        {
            throw new KeyNotFoundException($"Feature with ID '{id}' not found.");
        }

        return feature;
    }

    public async Task<Feature> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var feature = await _repository.GetByNameAsync(name, includeDeleted: false, cancellationToken);
        if (feature == null)
        {
            throw new KeyNotFoundException($"Feature with name '{name}' not found.");
        }

        return feature;
    }

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var existingFeature = await _repository.GetByNameAsync(name, includeDeleted: true, cancellationToken);
        if (existingFeature != null)
        {
            throw new InvalidOperationException($"A feature with the name '{name}' already exists.");
        }

        var newFeature = new Feature
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        await _repository.AddAsync(newFeature, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var feature = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (feature == null)
        {
            throw new KeyNotFoundException($"Feature with ID '{id}' not found.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
    }
}

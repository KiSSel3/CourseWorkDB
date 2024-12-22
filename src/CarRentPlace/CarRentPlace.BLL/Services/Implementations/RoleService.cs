using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;

    public RoleService(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAllAsync(includeDeleted: false, cancellationToken);
    }

    public async Task<Role> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID '{id}' not found.");
        }

        return role;
    }

    public async Task<Role> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var role = await _repository.GetByNameAsync(name, includeDeleted: false, cancellationToken);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with name '{name}' not found.");
        }

        return role;
    }

    public async Task CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        var existingRole = await _repository.GetByNameAsync(name, includeDeleted: true, cancellationToken);
        if (existingRole != null)
        {
            throw new InvalidOperationException($"A role with the name '{name}' already exists.");
        }

        var newRole = new Role
        {
            Id = Guid.NewGuid(),
            Name = name
        };

        await _repository.AddAsync(newRole, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await _repository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID '{id}' not found.");
        }

        await _repository.DeleteAsync(id, cancellationToken);
    }
}

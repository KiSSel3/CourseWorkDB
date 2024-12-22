using DriveType = CarRentPlace.Domain.Entities.DriveType;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface IDriveTypeService
{
    Task<IEnumerable<DriveType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<DriveType> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(string name, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

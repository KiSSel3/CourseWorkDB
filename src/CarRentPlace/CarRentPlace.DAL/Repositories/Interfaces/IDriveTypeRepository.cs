using DriveType = CarRentPlace.Domain.Entities.DriveType;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface IDriveTypeRepository : IBaseRepository<DriveType>
{
    Task<DriveType> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default);
}

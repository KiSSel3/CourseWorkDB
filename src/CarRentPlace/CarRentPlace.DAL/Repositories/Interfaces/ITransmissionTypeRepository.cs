using CarRentPlace.Domain.Entities;

namespace CarRentPlace.DAL.Repositories.Interfaces;

public interface ITransmissionTypeRepository : IBaseRepository<TransmissionType>
{
    Task<TransmissionType> GetByNameAsync(string name, bool includeDeleted = false, CancellationToken cancellationToken = default);
}
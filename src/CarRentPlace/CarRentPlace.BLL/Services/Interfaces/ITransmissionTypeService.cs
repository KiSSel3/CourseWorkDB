using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface ITransmissionTypeService
{
    Task<IEnumerable<TransmissionType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TransmissionType> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task CreateAsync(string name, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

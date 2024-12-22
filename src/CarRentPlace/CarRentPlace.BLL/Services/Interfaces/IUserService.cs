using System.Security.Claims;
using CarRentPlace.BLL.ViewModels.Authorization;
using CarRentPlace.Domain.Entities;

namespace CarRentPlace.BLL.Services.Interfaces;

public interface IUserService
{
    Task<ClaimsIdentity> LoginAsync(LoginViewModel viewModel, CancellationToken cancellationToken = default);
    Task<ClaimsIdentity> RegisterAsync(RegisterViewModel viewModel, CancellationToken cancellationToken = default);
    
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
using System.Security.Claims;
using CarRentPlace.BLL.Services.Interfaces;
using CarRentPlace.BLL.ViewModels.Authorization;
using CarRentPlace.DAL.Repositories.Interfaces;
using CarRentPlace.Domain.Entities;
using CarRentPlace.Domain.Models;

namespace CarRentPlace.BLL.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<ClaimsIdentity> LoginAsync(LoginViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(viewModel.Email, includeDeleted: false, cancellationToken);
        if (user is null || !VerifyPassword(viewModel.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var roles = await GetUserRolesAsync(user.Id, cancellationToken);
        
        return GenerateClaimsIdentity(user, roles);
    }

    public async Task<ClaimsIdentity> RegisterAsync(RegisterViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByEmailAsync(viewModel.Email, includeDeleted: false, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            Email = viewModel.Email,
            PhoneNumber = viewModel.PhoneNumber,
            PasswordHash = HashPassword(viewModel.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(newUser, cancellationToken);

        var defaultRole = await _roleRepository.GetByNameAsync("User", includeDeleted: false, cancellationToken);
        if (defaultRole != null)
        {
            await _userRoleRepository.AddAsync(new UserRole
            {
                UserId = newUser.Id,
                RoleId = defaultRole.Id
            }, cancellationToken);
        }

        var roles = await GetUserRolesAsync(newUser.Id, cancellationToken);
        
        return GenerateClaimsIdentity(newUser, roles);
    }

    public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    private async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userRoles = await _userRoleRepository.GetByUserIdAsync(userId, includeDeleted: false, cancellationToken);
        var roles = new List<Role>();

        foreach (var userRole in userRoles)
        {
            var role = await _roleRepository.GetByIdAsync(userRole.RoleId, includeDeleted: false, cancellationToken);
            if (role != null)
            {
                roles.Add(role);
            }
        }

        return roles;
    }
    
    private ClaimsIdentity GenerateClaimsIdentity(User user, IEnumerable<Role> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        return new ClaimsIdentity(
            claims,
            "Authentication",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
    }
    
    private string HashPassword(string password)
    {
        return password;
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return password == hashedPassword;
    }
}
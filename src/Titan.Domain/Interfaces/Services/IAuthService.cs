using Titan.Domain.Models;

namespace Titan.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<AuthToken> GetTokenAsync(string username, string password);
    
    Task<AuthToken> RefreshTokenAsync(string refreshToken);
    
    Task<bool> CreateUserAsync(string email, string password, string firstName, string lastName);
    
    Task<bool> AssignRolesToUserAsync(string userId, IEnumerable<string> roleNames);
}
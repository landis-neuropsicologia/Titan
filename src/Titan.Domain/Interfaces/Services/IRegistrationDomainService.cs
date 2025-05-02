using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Domain.Interfaces.Services;

public interface IRegistrationDomainService
{
    Task<UserBase> RegisterPersonAsync(Email email, Name name, bool registeredViaSocialMedia, SocialMediaProvider provider);
    
    Task<UserBase> RegisterCompanyAsync(Email email, Name companyName, DomainName domain);
    
    Task<bool> ActivateAccountAsync(Guid userId, ActivationKey key);
}
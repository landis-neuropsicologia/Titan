using Titan.Application.DTOs;
using Titan.Application.DTOs.Company;

namespace Titan.Application.Interfaces;

public interface IRegistrationService
{
    Task<RegistrationResponse> RegisterCompanyAsync(CreateCompanyRequest request, CancellationToken token);
}

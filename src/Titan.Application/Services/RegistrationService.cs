using Titan.Application.DTOs;
using Titan.Application.DTOs.Company;
using Titan.Application.Interfaces;
using Titan.Application.Mappers;
using Titan.Domain.Entities;
using Titan.Domain.Entities.User;
using Titan.Domain.Interfaces.Repositories;
using Titan.Domain.Interfaces.Services;
using Titan.Shared.Repositories;

namespace Titan.Application.Services;

public sealed class RegistrationService(IUserRepository userRepository, ICompanyRepository companyRepository, IUnitOfWork unitOfWork, IEmailService emailService) : IRegistrationService
{
    public async Task<RegistrationResponse> RegisterCompanyAsync(CreateCompanyRequest request, CancellationToken token)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.ContactEmail, token);

        if (existingUser is not null)
        {
            return new RegistrationResponse(false, "");
        }

        var company = await companyRepository.GetByNameAsync(request.CompanyName);

        if (company is not null)
        {
            return new RegistrationResponse(false, "");
        }

        company = request.MapTo();
        
        await companyRepository.AddAsync(company, token);

        // Criar novo usuário empresa
        var user = request.MapTo(company.Id);

        // Primeiro usuário da empresa é automaticamente administrador
        if (await userRepository.CountByCompanyIdAsync(company.Id, token) == 0)
        {
            user.MakeAdministrator();
        }

        // Salvar usuário
        await userRepository.AddAsync(user, token);
        await unitOfWork.SaveChangesAsync(token);

        // Enviar e-mail de ativação
        await SendActivationEmailAsync(company);

        return new RegistrationResponse(true, "Registration successful. Please check your email to activate your account.", company.Id, company.ActivationKey);
    }

    private async Task SendActivationEmailAsync(Company company)
    {
        var activationLink = $"https://yourapp.com/activate?userId={ company.Id }&key={ company.ActivationKey }";

        string subject = "Activate Your Account";
        string body = $@"
                <html>
                <body>
                    <h2>Welcome to Our Application!</h2>
                    <p>Thank you for registering. Please click the link below to activate your account:</p>
                    <p><a href='{activationLink}'>Activate Account</a></p>
                    <p>If you didn't create this account, please ignore this email.</p>
                </body>
                </html>";

        var admin = company.Users.FirstOrDefault(w => w.IsAdministrator());

        await emailService.SendEmailAsync(admin.Email, subject, body, true);
    }
}

namespace Titan.Domain.Interfaces.Services;

/// <summary>
/// Interface para o serviço de envio de emails
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Envia um email
    /// </summary>
    /// <param name="to">Endereço de email do destinatário</param>
    /// <param name="subject">Assunto do email</param>
    /// <param name="body">Corpo do email</param>
    /// <param name="isHtml">Indica se o corpo do email está em formato HTML</param>
    /// <returns>Task que representa a operação assíncrona</returns>
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);

    /// <summary>
    /// Envia um email para vários destinatários
    /// </summary>
    /// <param name="toAddresses">Lista de endereços de email dos destinatários</param>
    /// <param name="subject">Assunto do email</param>
    /// <param name="body">Corpo do email</param>
    /// <param name="isHtml">Indica se o corpo do email está em formato HTML</param>
    /// <returns>Task que representa a operação assíncrona</returns>
    Task SendEmailsAsync(IEnumerable<string> toAddresses, string subject, string body, bool isHtml = false);

    /// <summary>
    /// Envia um email de boas-vindas
    /// </summary>
    /// <param name="to">Endereço de email do destinatário</param>
    /// <returns>Task que representa a operação assíncrona</returns>
    Task SendWelcomeEmailAsync(string to);

    /// <summary>
    /// Envia um email de recuperação de senha
    /// </summary>
    /// <param name="to">Endereço de email do destinatário</param>
    /// <param name="resetLink">Link para resetar a senha</param>
    /// <returns>Task que representa a operação assíncrona</returns>
    Task SendPasswordResetEmailAsync(string to, string resetLink);
}


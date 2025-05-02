using Titan.Domain.Interfaces.Services;

namespace Titan.Infrastructure.Emails;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _senderEmail;
    private readonly string _senderName;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _smtpServer = _configuration["EmailSettings:SmtpServer"];
        _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
        _smtpUsername = _configuration["EmailSettings:SmtpUsername"];
        _smtpPassword = _configuration["EmailSettings:SmtpPassword"];
        _senderEmail = _configuration["EmailSettings:SenderEmail"];
        _senderName = _configuration["EmailSettings:SenderName"];

        if (string.IsNullOrEmpty(_smtpServer) || string.IsNullOrEmpty(_smtpUsername) ||
            string.IsNullOrEmpty(_smtpPassword) || string.IsNullOrEmpty(_senderEmail))
        {
            throw new InvalidOperationException("Email configuration is missing or incomplete");
        }
    }

    /// <inheritdoc />
    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        try
        {
            var message = new MailMessage
            {
                From = new MailAddress(_senderEmail, _senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            message.To.Add(to);

            using var client = CreateSmtpClient();
            await client.SendMailAsync(message);

            _logger.LogInformation("Email sent successfully to {Recipient}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", to);
            throw new EmailException("Failed to send email", ex);
        }
    }

    /// <inheritdoc />
    public async Task SendEmailsAsync(IEnumerable<string> toAddresses, string subject, string body, bool isHtml = false)
    {
        try
        {
            var message = new MailMessage
            {
                From = new MailAddress(_senderEmail, _senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            foreach (var address in toAddresses)
            {
                message.To.Add(address);
            }

            using var client = CreateSmtpClient();

            await client.SendMailAsync(message);

            _logger.LogInformation("Email sent successfully to {RecipientCount} recipients", toAddresses.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to multiple recipients");

            throw new EmailException("Failed to send email to multiple recipients", ex);
        }
    }

    /// <inheritdoc />
    public async Task SendWelcomeEmailAsync(string to)
    {
        string subject = "Welcome to Our Application";
        string body = @"
                <html>
                <body>
                    <h2>Welcome to Our Application!</h2>
                    <p>Thank you for joining us. Your account is now active.</p>
                    <p>You can now log in to your account and start using our services.</p>
                    <p>If you have any questions, please don't hesitate to contact our support team.</p>
                </body>
                </html>";

        await SendEmailAsync(to, subject, body, true);
    }

    /// <inheritdoc />
    public async Task SendPasswordResetEmailAsync(string to, string resetLink)
    {
        string subject = "Password Reset Request";
        string body = $@"
                <html>
                <body>
                    <h2>Password Reset Request</h2>
                    <p>We received a request to reset your password. Please click the link below to reset your password:</p>
                    <p><a href='{resetLink}'>Reset Password</a></p>
                    <p>If you didn't request a password reset, please ignore this email.</p>
                    <p>This link will expire in 24 hours.</p>
                </body>
                </html>";

        await SendEmailAsync(to, subject, body, true);
    }

    /// <summary>
    /// Cria e configura um cliente SMTP para envio de emails
    /// </summary>
    /// <returns>Cliente SMTP configurado</returns>
    private SmtpClient CreateSmtpClient()
    {
        var client = new SmtpClient(_smtpServer, _smtpPort)
        {
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            EnableSsl = true
        };

        return client;
    }
}

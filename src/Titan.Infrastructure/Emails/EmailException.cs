namespace Titan.Infrastructure.Emails;

/// <summary>
/// Exceção lançada quando ocorre um erro no envio de email
/// </summary>
public class EmailException : Exception
{
    public EmailException(string message) : base(message)
    {
    }

    public EmailException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
namespace Titan.Domain.Models;

public sealed record AuthToken(string AccessToken, string RefreshToken, int ExpiresIn, string TokenType);

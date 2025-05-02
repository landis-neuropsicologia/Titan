namespace Titan.Application.DTOs;

public sealed record RegistrationResponse(bool Success, string Message, Guid? UserId = null, string ActivationKey = null);
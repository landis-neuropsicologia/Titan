using Titan.Domain.ValueObjects;

namespace Titan.Application.DTOs.Website;

public sealed record ProfessionalResponse(string Name, string Email, string Specialty, string Description, string Image, string Curriculum);
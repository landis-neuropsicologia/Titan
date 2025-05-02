using Titan.Application.DTOs.CreateUser;
using Titan.Domain.Entities.User;
using Titan.Domain.ValueObjects;

namespace Titan.Application.Mappers;

public static class StaffUserMapper
{
    #region Create StaffUser

    internal static StaffUser MapTo(this CreateStaffUserRequest request)
    {
        var email = Email.Create(request.Email);
        var name = Name.Create(request.Name);
        var employeeId = EmployeeId.Create("");

        return new StaffUser(email, name, employeeId, request.Department);
    }

    internal static CreateStaffUserResponse MapFrom(this CreateStaffUserResponse response, StaffUser user)
    {
        response.Id = user.Id;
        response.EmployeeId = user.EmployeeId.Value;
        response.Name = user.Name.Value;
        response.Email = user.Email.Value;
        response.Department = user.Department;
        response.CreatedAt = user.CreatedAt;

        return response;
    }

    #endregion
}

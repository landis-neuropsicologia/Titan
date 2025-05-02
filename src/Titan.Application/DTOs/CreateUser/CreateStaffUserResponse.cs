using Titan.Application.Mappers;
using Titan.Domain.Entities.User;

namespace Titan.Application.DTOs.CreateUser;

public sealed class CreateStaffUserResponse
{
    #region Properties

    public Guid Id { get; internal set; }

    public string EmployeeId { get; internal set; }

    public string Name { get; internal set; }

    public string Email { get; internal set; }

    public string Department { get; internal set; }

    public DateTime CreatedAt { get; internal set; }

    #endregion

    #region C'tor

    private CreateStaffUserResponse() { }

    #endregion

    #region Factory

    public static CreateStaffUserResponse Create(StaffUser user)
    {
        var response = new CreateStaffUserResponse().MapFrom(user);

        return response;
    }

    #endregion
}


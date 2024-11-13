using Domain.Enums;

namespace Application.Contracts;

public interface IUserServiceFactory
{
    IUserService GetUserService(UserType userType);
}
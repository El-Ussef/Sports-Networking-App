using Domain.Entities;
using Domain.Enums;

namespace Application.Contracts;

public interface IUserService
{
    ValueTask<AppUser> Create(AppUser user);

    ValueTask<AppUser> Update(AppUser user);

    //ValueTask<AppUser> GetUser(UserType type,Guid refId);


}
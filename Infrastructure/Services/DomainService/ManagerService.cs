using Application.Contracts;
using Domain.Entities;

namespace Infrastructure.Identity.Services;

public class ManagerService : IUserService
{
    private readonly IApplicationDbContext _dbContext;

    public ManagerService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async  ValueTask<AppUser> Create(AppUser user)
    {
        var manager = new Manager
        {
            IsValid = false,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ThirdName = user.ThirdName,
            CityId = user.CityId,
            Email = user.Email,
            IsActive = false,
            UserType = user.UserType,
            SpecialityId = user.SpecialityId,
            JobTitle = user.JobTitle,
            PhoneNumber = user.PhoneNumber,
            ProfilePicturePath = user.ProfilePicturePath,
            RefId = user.RefId,
            Address = user.Address,
            Birthdate = user.Birthdate,
            PhotoTwo = user.PhotoTwo,
            PhotoOne = user.PhotoOne,
            Gender = user.Gender,
            Experience = user.Experience
        };
        await _dbContext.AppUsers.AddAsync(manager);
        return manager;
    }
    
    public ValueTask<AppUser> Update(AppUser user)
    {
        throw new NotImplementedException();
    }
}
using Application.Contracts;
using Domain.Entities;

namespace Infrastructure.Identity.Services;

public class AthleteService : IUserService
{
    private readonly IApplicationDbContext _dbContext;

    public AthleteService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    //TODO:Add DbContext, or create a abstract class for that l;
    public async ValueTask<AppUser> Create(AppUser user)
    {
        var athlete = new Athlete()
        {
            IsValid = false,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ThirdName = user.ThirdName,
            Height = user.Height,
            Weight = user.Weight,
            CareerStart = user.CareerStart,
            CityId = user.CityId,
            Email = user.Email,
            IsActive = false,
            UserType = user.UserType,
            SportId = user.SportId,
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
        await _dbContext.AppUsers.AddAsync(athlete);
        return athlete;
    }
    
    public async ValueTask<AppUser> Update(AppUser user)
    {
        var athlete = new Athlete()
        {
            IsValid = false,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ThirdName = user.ThirdName,
            CityId = user.CityId,
            Email = user.Email,
            IsActive = false,
            UserType = user.UserType,
            SportId = user.SportId,
            SpecialityId = user.SpecialityId,
            JobTitle = user.JobTitle,
            PhoneNumber = user.PhoneNumber,
            ProfilePicturePath = user.ProfilePicturePath,
            RefId = user.RefId
        };
         _dbContext.AppUsers.Update(athlete);
        return athlete;
    }
}
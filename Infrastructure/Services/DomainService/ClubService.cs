using Application.Contracts;
using Domain.Entities;

namespace Infrastructure.Identity.Services;

public class ClubService : IUserService
{
    private readonly IApplicationDbContext _dbContext;

    public ClubService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async ValueTask<AppUser> Create(AppUser user)
    {
        var club = new Club()
        {
            IsValid = false,
            CityId = user.CityId,
            Email = user.Email,
            IsActive = false,
            SportId = user.SportId,
            OrganisationName = user.OrganisationName,
            SpecialityId = user.SpecialityId,
            UserType = user.UserType,
            PhoneNumber = user.PhoneNumber,
            ProfilePicturePath = user.ProfilePicturePath,
            RefId = user.RefId,
            Address = user.Address,
            PhotoTwo = user.PhotoTwo,
            PhotoOne = user.PhotoOne,
        };
        await _dbContext.AppUsers.AddAsync(club);
        return club;
    }
    public async ValueTask<AppUser> Update(AppUser user)
    {
        var club = new Club()
        {
            IsValid = false,
            CityId = user.CityId,
            Email = user.Email,
            IsActive = false,
            SportId = user.SportId,
            OrganisationName = user.OrganisationName,
            SpecialityId = user.SpecialityId,
            UserType = user.UserType,
            PhoneNumber = user.PhoneNumber,
            ProfilePicturePath = user.ProfilePicturePath,
            RefId = user.RefId
        };
        await _dbContext.AppUsers.AddAsync(club);
        return club;
    }
}
using Application.Contracts;
using Domain.Entities;

namespace Infrastructure.Identity.Services;

public class MedicalAndHealthService : IUserService
{
    private readonly IApplicationDbContext _dbContext;

    public MedicalAndHealthService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async ValueTask<AppUser> Create(AppUser user)
    {
        var medicalStaff = new MedicalStaff()
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
        await _dbContext.AppUsers.AddAsync(medicalStaff);
        return medicalStaff;
    }
    
    public async ValueTask<AppUser> Update(AppUser user)
    {
        var medicalStaff = new MedicalStaff()
        {
            IsValid = false,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ThirdName = user.ThirdName,
            CityId = user.CityId,
            Email = user.Email,
            IsActive = false,
            UserType = user.UserType,
            JobTitle = user.JobTitle,
            PhoneNumber = user.PhoneNumber,
            ProfilePicturePath = user.ProfilePicturePath,
            RefId = user.RefId
        };
        await _dbContext.AppUsers.AddAsync(medicalStaff);
        return medicalStaff;
    }
}
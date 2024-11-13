using Application.Common.Helpers;
using Application.Common.Models;
using Application.Contracts;
using Application.Features.Registration;
using Application.Validation;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Identity.Entities;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser?> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenGenerator;
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserServiceFactory _userServiceFactory;
    private readonly IFileStorageService _fileStorageService;
    private readonly UploadFileHelper _uploadFileHelper;

    public IdentityService(IMapper mapper,
        UserManager<ApplicationUser?> userManager,
        IJwtTokenService jwtTokenGenerator,
        IApplicationDbContext dbContext,
        IUserServiceFactory _userServiceFactory,
        IFileStorageService fileStorageService
        )
    {
        _mapper = mapper;
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _dbContext = dbContext;
        this._userServiceFactory = _userServiceFactory;
        _fileStorageService = fileStorageService;
    }
    
    public async Task<AppUser> GetByUserName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return _mapper.Map<AppUser>(user);
    }

    public async Task<string?> CheckUserCredentials(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            return user.Id;
        }
        //var user = await _signInManager.PasswordSignInAsync(username, password, true, false);

        return string.Empty;
    }

    public async Task<AppUser> GetUserByIdAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return _mapper.Map<AppUser>(user);
    }

    public async Task<Result<AppUser, ValidationFailed>> RegisterAppUserAsync(RegistrationRequest data,string password)
    {
        try
        {
                var speciality = await _dbContext.Specialities.FirstOrDefaultAsync(s=> s.Id == data.SpecialityId);

                if (speciality is null)
                {
                    var failure = new ValidationFailure("IdentityService", "Failed to find speciality");
                    return new Result<AppUser, ValidationFailed>(new ValidationFailed(failure));
                }
                
                var appUser = new AppUser()
                {
                    FirstName = data.FirstName.Trim(),
                    LastName = data.LastName.Trim(),
                    //ThirdName = data.ThirdName?.Trim(),
                    PhoneNumber = data.PhoneNumber?.Trim(),
                    Email = data.Email.Trim(),
                    CityId = data.CityId,
                    SpecialityId = data.SpecialityId,
                    SportId = data.SportId,
                    UserType = speciality.UserType != null ? speciality.UserType : UserType.BaseType,
                    OrganisationName = data.OrganisationName?.Trim(),
                    JobTitle = data.JobTitle,
                    Birthdate = data.Birthdate != null ? DateTime.SpecifyKind(data.Birthdate.Value, DateTimeKind.Utc) : null,
                    CareerStart = data.CareerStart,
                    Gender = data.Gender ?? CoreEnums.Gender.Na,
                    Address = data.Address,
                    IsActive = false,
                    IsValid = false,
                };
                
                var user = await CreateApplicationUserUserAsync(appUser,password);
                if (user is null)
                {
                    var failure = new ValidationFailure("IdentityService", "Failed to create user");
                    return new Result<AppUser, ValidationFailed>(new ValidationFailed(failure));
                }
                
                if (data.ProfilePhoto != null)
                {
                    user.ProfilePicturePath = await SaveImagesAsync(data.ProfilePhoto,user.UserType.ToString(),user.RefId.ToString());
                }
                
                if (data.PhotoOne != null)
                {
                    user.PhotoOne = await SaveImagesAsync(data.PhotoOne,user.UserType.ToString(),user.RefId.ToString());
                }
                
                if (data.PhotoTwo != null)
                {
                    user.PhotoTwo =  await SaveImagesAsync(data.PhotoTwo,user.UserType.ToString(),user.RefId.ToString());
                }

                var userService = _userServiceFactory.GetUserService(user.UserType);
                var createdUser = await userService.Create(user) as AppUser;

                return await _dbContext.SaveChangesAsync() > 0 ? createdUser : null ;

        }
        catch (Exception e)
        {
            var failure = new ValidationFailure("RegistrationCommand", e.Message);
            return new Result<AppUser, ValidationFailed>(new ValidationFailed(failure));
        }
    }

    public async Task UpdateAsync(AppUser user)
    {
        var appUser = await _userManager.FindByEmailAsync(user.Email);
        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        
        if (appUser == null)
            throw new ArgumentNullException(nameof(AppUser), user.Email);

        _dbContext.AppUsers.Update(user);
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
    {
        var appUser =  await _userManager.FindByEmailAsync(user.Email);
        
        
        if (appUser == null)
            throw new ArgumentNullException(nameof(AppUser), user.Email);
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

        return token;
    }


    public Task<AppUser> UpdateUserAsync(string userId, AppUser updateduser)
    {
        throw new NotImplementedException();
    }

    public async Task<AppUser> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        //TODO:create a NotFoundException 
        if (user == null)
            throw new ArgumentNullException(nameof(AppUser), email);

        return _mapper.Map<AppUser>(user);
    }

    public Task<List<string>> GetUserClaimesAsync(int userId, string claimType)
    {
        throw new NotImplementedException();
    }
    
    private async Task<AppUser> CreateApplicationUserUserAsync(AppUser appuser,string password)
    {
        try
        {
            
            var user = _mapper.Map<ApplicationUser>(appuser);
            user.UserName = appuser.Email;
            user.Id = Guid.NewGuid().ToString();

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                appuser.RefId = Guid.Parse(user.Id);
                return appuser;
            }
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private async Task<string> SaveImagesAsync(FileUpload fileUpload,string folder, string? uniqueFolderName = null)
    {
        var path = Path.Combine(folder, uniqueFolderName);
            
        var filePath = await _fileStorageService.SaveFileWithRelativePathAsync(fileUpload.Data,path, fileUpload.FileName);
            
        // Extract the filename from the filePath
        var fileName = Path.GetFileName(filePath);

        // Return a relative URL
        return $"Images/{path}/{fileName}";
        //return filePath;
    }
    
}
using Application.Common.Models;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Registration;

//TODO: TO REMOVE 
public class RegistrationCommand : IRequest<Result<AppUser,ValidationFailed>>
{
    public RegistrationRequest Data { get; set; }
    
    class RegistrationCommandHandler: IRequestHandler<RegistrationCommand,Result<AppUser,ValidationFailed>>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IUserServiceFactory _userServiceFactory;
        private readonly IApplicationDbContext _dbContext;
        private readonly IFileStorageService _fileStorageService;

        public RegistrationCommandHandler(
            IIdentityService identityService,
            IMapper mapper,
            IUserServiceFactory userServiceFactory,
            IApplicationDbContext dbContext,
            IFileStorageService fileStorageService
            )
        {
            _identityService = identityService;
            _mapper = mapper;
            _userServiceFactory = userServiceFactory;
            _dbContext = dbContext;
            _fileStorageService = fileStorageService;
        }
        
        public async Task<Result<AppUser,ValidationFailed>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var data = request.Data;
            
            try
            {
                var speciality = await _dbContext.Specialities.FirstOrDefaultAsync(s=> s.Id == data.SpecialityId,cancellationToken);

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
                    Birthdate = data.Birthdate,
                    Gender = data.Gender ?? CoreEnums.Gender.Na,
                    Experience = data.Experience,
                    IsActive = false,
                    IsValid = false,
                };

                var user = appUser;
                    //await _identityService.CreateApplicationUserUserAsync(appUser,data.Password);
                // if (user is null)
                // {
                //     var failure = new ValidationFailure("IdentityService", "Failed to create user");
                //     return new Result<AppUser, ValidationFailed>(new ValidationFailed(failure));
                // }
                string profilePhotoUrl = null;
                
                if (data.ProfilePhoto != null)
                {
                    user.ProfilePicturePath  = await SaveImagesAsync(data.ProfilePhoto,user.UserType.ToString(),user.RefId.ToString());
                }
                
                //user.ProfilePicturePath = profilePhotoUrl;

                if (data.PhotoOne != null)
                {
                    user.PhotoOne = await SaveImagesAsync(data.PhotoOne, user.UserType.ToString(),
                        user.RefId.ToString());
                }
                
                if (data.PhotoTwo != null)
                {
                    user.PhotoTwo = await SaveImagesAsync(data.PhotoTwo, user.UserType.ToString(),
                        user.RefId.ToString());
                }
                
                var userService = _userServiceFactory.GetUserService(user.UserType);
                var athlete = await userService.Create(user) as AppUser;

                return await _dbContext.SaveChangesAsync(cancellationToken) > 0 ? athlete : null ;

            }
            catch (Exception e)
            {
                var failure = new ValidationFailure("RegistrationCommand", e.Message);
                return new Result<AppUser, ValidationFailed>(new ValidationFailed(failure));
            }
            
        }
        //TODO: make it a service
        private async Task<string> SaveImagesAsync(FileUpload fileUpload,string folder, string? uniqueFolderName = null)
        {
            var path = Path.Combine(folder, uniqueFolderName);
            try
            {
                var filePath = await _fileStorageService.SaveFileWithRelativePathAsync(fileUpload.Data,path, fileUpload.FileName);
            
                // Extract the filename from the filePath
                var fileName = Path.GetFileName(filePath);

                // Return a relative URL
                return $"Images/{path}/{fileName}";
            }
            catch (Exception)
            {
                return null;
            }

            //return filePath;
        }
    }
}
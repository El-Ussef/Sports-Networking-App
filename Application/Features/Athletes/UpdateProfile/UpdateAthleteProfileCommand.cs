using Application.Common.Models;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Athletes.UpdateProfile;

public class UpdateAthleteProfileCommand : IRequest<Result<bool,ValidationFailed>>
{
    public Guid Id { get; set; }
    public ProfileDto Data { get; set; }
    
    public class UpdateAthleteProfileCommandHandler : IRequestHandler<UpdateAthleteProfileCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public UpdateAthleteProfileCommandHandler(
            IApplicationDbContext context,
            IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }
        
        public async Task<Result<bool,ValidationFailed>> Handle(UpdateAthleteProfileCommand request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            try
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId == request.Id,cancellationToken);
                if (user is null)
                {
                    //TODO:return object that holds the messages of not found 
                    var failure = new ValidationFailure("AppUsers", "Failed to find user");
                    return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
                }
                
                var data = request.Data;
            
                if (data.FullName != string.Empty)
                    user.FirstName = data.FullName.Trim();
            
                if (data.JobTitle != string.Empty)
                    user.JobTitle = data.JobTitle.Trim();
                
                if (data.Presentation != string.Empty)
                    user.Presentation = data.Presentation.Trim();

                if (data.PhoneNumber != string.Empty)
                    user.PhoneNumber = data.PhoneNumber.Trim();
                
                if (data.Email != string.Empty)
                    user.Email = data.Email.Trim();
                
                if (data.CityId > 0)
                    user.CityId = data.CityId;
            
                string profilePhotoUrl = null;
                
                if (data.CoverImage != null)
                {
                    profilePhotoUrl = await SaveProfilePhotoAsync(data.CoverImage,user);
                }

                user.ProfilePicturePath = profilePhotoUrl;
                
                //TODO: find a way to check if there has been an update 
                _context.AppUsers.Update(user);
            
                List<Service> services = new List<Service>();
            
                if (data.ServiceTypesIds.Any())
                {
                    foreach (var serviceTypesId in data.ServiceTypesIds)
                    {
                        services.Add( new Service
                        {
                            AppUserId = user.Id,
                            ServiceTypeId = serviceTypesId,
                        });
                    }
                }
                
                if (data.CustomService.Any())
                {
                    var customService = new Service
                    {
                        AppUserId = user.Id,
                        //CustomService = new List<string>()
                    };
                    
                    foreach (var service in data.CustomService)
                    {
                        //customService.CustomService.Add(service);
                        services.Add(customService);
                    }
                }
            
                List<Achievement> achievements = new List<Achievement>();
                foreach (var achievementDto in data.Achievements)
                {
                    achievements.Add(new Achievement()
                    {
                        Title = achievementDto.Title,
                        Description = achievementDto.Description,
                        DateAchieved = achievementDto.DateAchieved.ToUniversalTime(),
                        AthleteId = user.Id
                    });
                    
                }
            
                //TODO: with each call we should be check if he completed all the info to activate his account
                
                await _context.Achievements.AddRangeAsync(achievements, cancellationToken);
                await _context.Services.AddRangeAsync(services,cancellationToken);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception e)
            {
                var failure = new ValidationFailure("UpdateAthleteProfileCommand", e.Message);
                return new Result<bool, ValidationFailed>(new ValidationFailed(failure));
            }
        }
        
        private async Task<string> SaveProfilePhotoAsync(FileUpload fileUpload,AppUser user)
        {
            var path = Path.Combine(user.UserType.ToString(), user.RefId.ToString());
            
            var filePath = await _fileStorageService.SaveFileWithRelativePathAsync(fileUpload.Data,path, fileUpload.FileName);
            
            // Extract the filename from the filePath
            var fileName = Path.GetFileName(filePath);

            // Return a relative URL
            return $"Images/{path}/{fileName}";
            return filePath;
        }
    }
}
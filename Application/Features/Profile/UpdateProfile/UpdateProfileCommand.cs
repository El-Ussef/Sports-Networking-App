using Application.Common.Helpers;
using Application.Common.Models;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.UpdateProfile;

public class UpdateProfileCommand : IRequest<Result<bool,ValidationFailed>>
{
    public Guid RefId { get; set; }
    public ProfileRequestDto Data { get; set; }

    class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UploadFileHelper _uploadFileHelper;
        
        public UpdateProfileCommandHandler(
            IApplicationDbContext context,
            UploadFileHelper uploadFileHelper)
        {
            _context = context;
            _uploadFileHelper = uploadFileHelper;
        }
        public async Task<Result<bool, ValidationFailed>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        { 
            try
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId == request.RefId,cancellationToken);
                if (user is null)
                {
                    //TODO:return object that holds the messages of not found 
                    var failure = new ValidationFailure("AppUsers", "Failed to find user");
                    return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
                }
                
                var data = request.Data;
            
                if (!string.IsNullOrWhiteSpace(data.FirstName))
                    user.FirstName = data.FirstName.Trim();

                if (!string.IsNullOrWhiteSpace(data.CareerStart))
                    user.FirstName = data.CareerStart.Trim();
                
                if (!string.IsNullOrWhiteSpace(data.LastName))
                    user.FirstName = data.LastName.Trim();
                
                if (!string.IsNullOrWhiteSpace(data.OrganisationName))
                    user.FirstName = data.OrganisationName.Trim();
                
                if (!string.IsNullOrWhiteSpace(data.ThirdName))
                    user.FirstName = data.ThirdName.Trim();

                if (!string.IsNullOrWhiteSpace(data.JobTitle))
                    user.JobTitle = data.JobTitle.Trim();
                
                if (!string.IsNullOrWhiteSpace(data.Presentation))
                    user.Presentation = data.Presentation.Trim();

                if (!string.IsNullOrWhiteSpace(data.PhoneNumber))
                    user.PhoneNumber = data.PhoneNumber.Trim();
                
                if (!string.IsNullOrWhiteSpace(data.Email))
                    user.Email = data.Email.Trim();
                
                if (data.CityId is > 0)
                    user.CityId = data.CityId.Value;
            
                if (data.SportId is > 0)
                    user.SportId = data.SportId;
                
                if (data.NationalityId is > 0)
                    user.NationalityId = data.NationalityId;
                
                if (data.Birthdate != DateTime.MinValue && data.Birthdate is not null)
                    user.Birthdate = data.Birthdate.Value.ToUniversalTime();

                if (data.CoverImage != null)
                {
                    user.CoverImagePath = await _uploadFileHelper.SaveProfilePhotoAsync(data.CoverImage);
                }
                
                if (data.ProfilePicture != null)
                {
                    user.ProfilePicturePath = await _uploadFileHelper.SaveProfilePhotoAsync(data.ProfilePicture);
                }
                
                if (data.PhotoOne != null)
                {
                    user.PhotoOne = await _uploadFileHelper.SaveProfilePhotoAsync(data.PhotoOne);
                }
                
                if (data.PhotoTwo != null)
                {
                    user.PhotoTwo = await _uploadFileHelper.SaveProfilePhotoAsync(data.PhotoTwo);
                }
                
                _context.AppUsers.Update(user);
                
                var result = await _context.SaveChangesAsync(cancellationToken) > 0;
                return result;
            }
            catch (Exception e)
            {
                var failure = new ValidationFailure("UpdateAthleteProfileCommand", e.Message);
                return new Result<bool, ValidationFailed>(new ValidationFailed(failure));
            }
        }
        

    }
}
using Application.Common.Helpers;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.UpdateProfile;

public class UpdateImageGalleryCommand: IRequest<Result<bool,ValidationFailed>>
{
    public string RefId { get; set; }

    public ImageGalleryDto Data { get; set; }

    class UpdateImageGalleryCommandHandler : IRequestHandler<UpdateImageGalleryCommand,
        Result<bool, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UploadFileHelper _uploadFileHelper;

        public UpdateImageGalleryCommandHandler(IApplicationDbContext context, UploadFileHelper uploadFileHelper)
        {
            _context = context;
            _uploadFileHelper = uploadFileHelper;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(UpdateImageGalleryCommand request, CancellationToken cancellationToken)
        {
            
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.RefId.ToString() == request.RefId,cancellationToken);
            if (user is null)
            {
                //TODO:return object that holds the messages of not found 
                var failure = new ValidationFailure("AppUsers", "Failed to find user");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }

            try
            {
                var data = request.Data;

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
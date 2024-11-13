using Application.Common.Helpers;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.DeleteCommands;

public class DeleteImageCommand: IRequest<Result<bool,ValidationFailed>>
{
    public string RefId { get; set; }

    public ImageDeleteDto Data { get; set; }

    class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand,
        Result<bool, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UploadFileHelper _uploadFileHelper;

        public DeleteImageCommandHandler(IApplicationDbContext context, UploadFileHelper uploadFileHelper)
        {
            _context = context;
            _uploadFileHelper = uploadFileHelper;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
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

                if (data.ImageFields.Any(c=> c == ImageField.CoverImage))
                {
                    await _uploadFileHelper.DeleteFileAsync(user.CoverImagePath);
                    user.CoverImagePath = null;
                }
                
                if (data.ImageFields.Any(c=> c == ImageField.ProfilePhoto))
                {
                    await _uploadFileHelper.DeleteFileAsync(user.ProfilePicturePath);
                    user.ProfilePicturePath = null;
                }
                
                if (data.ImageFields.Any(c=> c == ImageField.PhotoOne))
                {
                    await _uploadFileHelper.DeleteFileAsync(user.PhotoOne);
                    user.PhotoOne =null;
                }
                
                if (data.ImageFields.Any(c=> c == ImageField.PhotoTwo))
                {
                    await _uploadFileHelper.DeleteFileAsync(user.PhotoTwo);
                    user.PhotoTwo = null;
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
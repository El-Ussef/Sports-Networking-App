using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Profile.GetProfileById;

public class GetProfileImagesQuery: IRequest<Result<ImageGalleryResponse,ValidationFailed>>
{
    public string Type { get; set; }

    public string RefId { get; set; }
    
    public class GetProfileImagesQueryHandler : IRequestHandler<GetProfileImagesQuery,Result<ImageGalleryResponse,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public GetProfileImagesQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<ImageGalleryResponse, ValidationFailed>> Handle(GetProfileImagesQuery request, CancellationToken cancellationToken)
        {

            if (!Enum.TryParse<UserType>(request.Type, true, out var userTypeEnum))
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user type");
                return new Result<ImageGalleryResponse, ValidationFailed>(new ValidationFailed(failure));
            }

            var user = await _context.AppUsers
                .Where(u => u.UserType == userTypeEnum && u.RefId.ToString() == request.RefId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user");
                return new Result<ImageGalleryResponse, ValidationFailed>(new ValidationFailed(failure));
            }

            ImageGalleryResponse result = new ImageGalleryResponse
            {
                ProfilePhoto = user.ProfilePicturePath,
                PhotoTwo = user.PhotoTwo,
                PhotoOne = user.PhotoOne,
                CoverImage = user.CoverImagePath
            };
            
            return result;
        }
    }
}
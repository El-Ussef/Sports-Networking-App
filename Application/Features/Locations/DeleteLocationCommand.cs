using Application.Common.Helpers;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Locations;

public class DeleteLocationCommand: IRequest<Result<bool,ValidationFailed>>
{
   public int Id { get; set; }

   public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, Result<bool, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UploadFileHelper _uploadFileHelper;

        public DeleteLocationCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            UploadFileHelper uploadFileHelper
        )
        {
            _context = context;
            _currentUserService = currentUserService;
            _uploadFileHelper = uploadFileHelper;
        }

        public async Task<Result<bool, ValidationFailed>> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            var userId = int.Parse(_currentUserService.Id);

            var location = await _context.Locations
                .FirstOrDefaultAsync(a => a.AppUserId == userId &&
                                          a.Id == request.Id, cancellationToken);

            if (location is null)
            {
                var failure = new ValidationFailure("UpdateLocationCommand", "Failed to find the location to update");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }
            

            // Handle image uploads
            if (location.LocationImages != null && location.LocationImages.Any())
            {
                await _uploadFileHelper.DeleteFilesAsync(location.LocationImages);
                // foreach (var locationImage in location.LocationImages)
                // {
                //     var url = await _uploadFileHelper.DeleteFileAsync(locationImage);
                // }
            }

            _context.Locations.Remove(location);

            return await _context.SaveChangesAsync(cancellationToken) > 0;

        }
    }
}
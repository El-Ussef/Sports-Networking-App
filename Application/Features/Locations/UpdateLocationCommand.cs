using Application.Common.Helpers;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Locations;

public class UpdateLocationCommand : IRequest<Result<bool,ValidationFailed>>
{
   public int Id { get; set; }
   
    public LocationUpdateVm LocationUpdateVm { get; set; }

    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, Result<bool, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UploadFileHelper _uploadFileHelper;

        public UpdateLocationCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            UploadFileHelper uploadFileHelper
        )
        {
            _context = context;
            _currentUserService = currentUserService;
            _uploadFileHelper = uploadFileHelper;
        }

        public async Task<Result<bool, ValidationFailed>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var userId = int.Parse(_currentUserService.Id);

            var data = request.LocationUpdateVm;
            
            var location = await _context.Locations
                .FirstOrDefaultAsync(a => a.AppUserId == userId &&
                                          a.Id == request.Id, cancellationToken);

            if (location is null)
            {
                var failure = new ValidationFailure("UpdateLocationCommand", "Failed to find the location to update");
                return new Result<bool,ValidationFailed>(new ValidationFailed(failure));
            }

            // Update basic properties
            location.LocationName = request.LocationUpdateVm.LocationName;
            location.Address = request.LocationUpdateVm.Address;
            location.Latitude = request.LocationUpdateVm.Latitude;
            location.Longitude = request.LocationUpdateVm.Longitude;
            location.OpeningHours = new OpeningHours
            {
                Start = TimeSpan.Parse(request.LocationUpdateVm.OpeningHoursStart),
                End = TimeSpan.Parse(request.LocationUpdateVm.OpeningHoursEnd)
            };

            // Handle image uploads
            if (request.LocationUpdateVm.LocationImages != null && request.LocationUpdateVm.LocationImages.Any())
            {
                location.LocationImages = new List<string>();

                foreach (var locationImage in data.LocationImages)
                {
                    var url = await _uploadFileHelper.SaveProfilePhotoAsync(locationImage);

                    location.LocationImages.Add(url);

                }
            }

            _context.Locations.Update(location);

            return await _context.SaveChangesAsync(cancellationToken) > 0;

        }
    }
}
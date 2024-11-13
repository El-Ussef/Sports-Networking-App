using Application.Common.Helpers;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Events;

public class CreateEventCommand: IRequest<Result<bool,ValidationFailed>>
{
    public CreateEventDto NewEvent { get; set; }
    
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UploadFileHelper _uploadFileHelper;

        public CreateEventCommandHandler(
            IApplicationDbContext context,
            UploadFileHelper uploadFileHelper
            )
        {
            _context = context;
            _uploadFileHelper = uploadFileHelper;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var data = request.NewEvent;

            var newEvent = new Event
            {
                Name = data.Name,
                CityId = data.CityId,
                Description = data.Description,
                EventDate = data.EventDate,
                IsActive = true,
                Latitude = data.Latitude,
                Longitude = data.Longitude
            };
            
            if (data.ProfilePicture != null)
            {
                newEvent.PhotoPath = await _uploadFileHelper.SaveProfilePhotoAsync(data.ProfilePicture);
            }
                
            if (data.EventImages != null && data.EventImages.Any())
            {
                foreach (var images in data.EventImages )
                {
                    var path = await _uploadFileHelper.SaveProfilePhotoAsync(images);
                    newEvent.EventImages.Add(path);
                }
            }
            
            await _context.Events.AddAsync(newEvent, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
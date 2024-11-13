using Application.Common.Helpers;
using Application.Common.Models;
using Application.Contracts;
using Application.Features.Achievements;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Domain.Entities;

namespace Application.Features.Locations;

public class CreateLocationCommand: IRequest<Result<bool,ValidationFailed>>
{
    public Location Data { get; set; }

    public List<FileUpload> LocationImages { get; set; }
    
    class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand,Result<bool,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UploadFileHelper _uploadFileHelper;

        public CreateLocationCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            UploadFileHelper uploadFileHelper
            )
        {
            _context = context;
            _currentUserService = currentUserService;
            _uploadFileHelper = uploadFileHelper;
        }
        
        public async Task<Result<bool, ValidationFailed>> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = request.Data;

            foreach (var locationImage in request.LocationImages)
            {
                var url = await _uploadFileHelper.SaveProfilePhotoAsync(locationImage);

                location.LocationImages.Add(url);

            }
            //save image here 
            await _context.Locations.AddAsync(location, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
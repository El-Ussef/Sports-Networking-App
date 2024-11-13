using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Locations;

public class GetLocationByIdQuery : IRequest<Result<LocationVm,ValidationFailed>>
{
    public int Id { get; set; }
    
    public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery,
        Result<LocationVm, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetLocationByIdQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService
            )
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Result<LocationVm, ValidationFailed>> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = int.Parse(_currentUserService.Id);


            var location = await _context.Locations
                .FirstOrDefaultAsync(a => a.AppUserId == userId && a.Id == request.Id, cancellationToken)
;
                
            if (location is null)
            {
                var failure = new ValidationFailure("GetLocationByIdQuery", "Failed to Find Location");
                return new Result<LocationVm, ValidationFailed>(new ValidationFailed(failure));
            }
            
            return new LocationVm()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Address = location.Address,
                OpeningHoursEnd = location.OpeningHours.End.ToString(@"hh\:mm"),
                OpeningHoursStart = location.OpeningHours.Start.ToString(@"hh\:mm"),
                LocationName = location.LocationName,
                LocationImages = location.LocationImages
            };
        }
    }
}
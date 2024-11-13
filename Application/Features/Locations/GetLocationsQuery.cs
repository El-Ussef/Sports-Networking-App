using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Locations;

public class GetLocationsQuery: IRequest<Result<IEnumerable<LocationVm>,ValidationFailed>>
{
    public int Id { get; set; }
    
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery,
        Result<IEnumerable<LocationVm>,ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetLocationsQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService
        )
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Result<IEnumerable<LocationVm>,ValidationFailed>> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var userId = int.Parse(_currentUserService.Id);
            
            var locations = await _context.Locations
                .Where(a => a.AppUserId == userId)
                .Select(c=> new LocationVm
                {
                    Id = c.Id,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude,
                    Address = c.Address,
                    OpeningHoursEnd = c.OpeningHours.End.ToString(@"hh\:mm"),
                    OpeningHoursStart = c.OpeningHours.Start.ToString(@"hh\:mm"),
                    LocationName = c.LocationName,
                    LocationImages = c.LocationImages
                })
                .ToListAsync(cancellationToken);


            return locations;
        }
    }
}
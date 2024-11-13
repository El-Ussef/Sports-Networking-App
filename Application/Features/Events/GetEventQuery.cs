using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Events;

public class GetEventQuery : IRequest<Result<EventDetailDto, ValidationFailed>>
{
    public int Id { get; set; }
    class GetEventQueryHandler : IRequestHandler<GetEventQuery,Result<EventDetailDto, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public GetEventQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<EventDetailDto, ValidationFailed>> Handle(GetEventQuery request, CancellationToken cancellationToken)
        {
            var tempEvent = await _context.Events.Include(e => e.City)
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (tempEvent is null)
            {
                var failure = new ValidationFailure("GetEventQuery", "Failed to Find Event");
                return new Result<EventDetailDto, ValidationFailed>(new ValidationFailed(failure));
            }

            var result = new EventDetailDto
            {
                Name = tempEvent!.Name,
                Description = tempEvent.Description,
                EventDate = tempEvent.EventDate,
                City = tempEvent.City.Label,
                ProfilePicture = tempEvent.PhotoPath,
                EventImages = tempEvent.EventImages,
                Longitude = tempEvent.Longitude,
                Latitude = tempEvent.Latitude
            };

            return result;

        }
    }
}
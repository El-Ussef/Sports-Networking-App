using Application.Common.Options;
using Application.Contracts;
using Application.Features.Profile;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Features.Events;

public class GetEventsByCriteriaQuery : IRequest<PagedResponse<IEnumerable<EventDto>>>
{
    public FilterCriteria Filter { get; set; }
    
    public class GetEventsByCriteriaQueryHandler : IRequestHandler<GetEventsByCriteriaQuery,PagedResponse<IEnumerable<EventDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly StorageOption _storageOptions;


        public GetEventsByCriteriaQueryHandler(IApplicationDbContext context, IOptions<StorageOption> storageOptions)
        {
            _context = context;
            _storageOptions = storageOptions.Value;
        }
        
        public async Task<PagedResponse<IEnumerable<EventDto>>> Handle(GetEventsByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;
            var query = _context.Events.AsQueryable();

            if (filter.CityId is > 0)
            {
                query = query.Where(u => u.CityId == filter.CityId);
            }
            
            var result = await query.Select(u => new EventDto
            {
                Id = u.Id,
                Name = u.Name.ToString(),
                Description = u.Description,
                City = u.City.Label,
                ProfilePicture = _storageOptions.BaseUrl + u.PhotoPath,

            }).ToPagedListAsync(filter.PageNumber, filter.PageSize);
            
            return new PagedResponse<IEnumerable<EventDto>>(result, filter.PageNumber, filter.PageSize);
        }
    }
}
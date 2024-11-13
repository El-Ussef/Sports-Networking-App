using Application.Contracts;
using Application.Wrappers;
using Domain.Enums;
using MediatR;

namespace Application.Features.Athletes.GetAthletes;

public class GetAthletesQuery: IRequest<PagedResponse<IEnumerable<AtheletListModelResponse>>>
{
    public FilterCriteria FilterCriteria { get; set; }
    
    public class GetAthletesQueryHandler : IRequestHandler<GetAthletesQuery,PagedResponse<IEnumerable<AtheletListModelResponse>>>
    {
        private readonly IApplicationDbContext _context;

        public GetAthletesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<PagedResponse<IEnumerable<AtheletListModelResponse>>> Handle(GetAthletesQuery request, CancellationToken cancellationToken)
        {
            var data = request.FilterCriteria;
            
            var param = new PagingParameter(data.PageNumber,data.PageSize);

            var query =  _context.AppUsers.AsQueryable();

            if (data.Age.HasValue)
            {
                query.Where(c => c.Age <= data.Age);
            }
            
            if (data.CityId.HasValue)
            {
                query.Where(c => c.Age == data.CityId);
            }
            //TODO:Sport should be by name also maybe
            if (data.SportId.HasValue)
            {
                query.Where(c => c.Age == data.SportId);
            }
            
            var result = await query
                .Where(c => c.UserType == UserType.Athlete)
                .Select(c => new AtheletListModelResponse
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    ImageUrl = c.ProfilePicturePath,
                    UserType = c.UserType.ToString()
                })
                .ToPagedListAsync(param.PageNumber, param.PageSize);
            
            
            return new PagedResponse<IEnumerable<AtheletListModelResponse>>(result, param.PageNumber, param.PageSize);

        }
    }
}
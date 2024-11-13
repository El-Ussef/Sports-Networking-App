using Application.Common.Extensions;
using Application.Common.Models;
using Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Sport.GetSports;

public class GetSportsByCriteriaQuery: IRequest<IEnumerable<IdValue<int>>>
{
    public SportsCriteria Criteria { get; set; }
    
    public class GetSportsByCriteriaQueryHandler : IRequestHandler<GetSportsByCriteriaQuery,IEnumerable<IdValue<int>>>
    {
        private readonly IApplicationDbContext _context;

        public GetSportsByCriteriaQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<IdValue<int>>> Handle(GetSportsByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var criteria = request.Criteria;

            var query =  _context.Sports.AsQueryable();
            
            if (criteria.SportTypeId is > 0)
            {
                query = query.Where(s => s.SportCategoryId == criteria.SportTypeId);
            }

            if (!criteria.SportName.IsNullOrWhiteSpace())
            {
                query = query.Where(s => s.Name.ToLower().Contains(criteria.SportName.ToLower()));

            }
            
            return await query
                .Select(c => new IdValue<int>()
                {
                    Id = c.Id,
                    Value = c.Name
                }).AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
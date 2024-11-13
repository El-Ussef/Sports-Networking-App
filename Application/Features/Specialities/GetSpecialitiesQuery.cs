using Application.Contracts;
using Application.Features.Country;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Specialities;

public class GetSpecialitiesQuery : IRequest<List<SpecialityDto>>
{
    public class GetSpecialitiesQueryHandler : IRequestHandler<GetSpecialitiesQuery,List<SpecialityDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetSpecialitiesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<SpecialityDto>> Handle(GetSpecialitiesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Specialities
                .AsNoTracking()
                .Select(c => new SpecialityDto
                {
                    Id = c.Id,
                    Value = c.Label
                })
                .ToListAsync(cancellationToken);
        }
    }
}
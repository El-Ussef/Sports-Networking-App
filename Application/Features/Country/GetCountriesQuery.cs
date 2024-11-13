using Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Country;

public class GetCountriesQuery : IRequest<List<CountryDto>>
{
    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery,List<CountryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCountriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Countries
                .AsNoTracking()
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    Value = c.Label,
                    Code = c.CountryCode
                })
                .ToListAsync(cancellationToken);
        }
    }
}
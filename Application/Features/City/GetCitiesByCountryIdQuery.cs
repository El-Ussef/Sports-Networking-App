using Application.Contracts;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.City;

public class GetCitiesByCountryIdQuery : IRequest<List<CityDto>>
{
    public int CountryId { get; set; }
    
    public class GetCitiesByCountryIdHandler: IRequestHandler<GetCitiesByCountryIdQuery,List<CityDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetCitiesByCountryIdHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<CityDto>> Handle(GetCitiesByCountryIdQuery request, CancellationToken cancellationToken)
        {
            var countryId = request.CountryId;

            var cities = await _context.Cities
                .Where(c => c.CountryId == countryId)
                .AsNoTracking()
                .Select(c => new CityDto
                {
                    Id = c.Id,
                    Value = c.Label
                }).ToListAsync(cancellationToken);
            
            return cities;
        }
    }
}
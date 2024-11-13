using Application.Common.Models;
using Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Services;

public class GetServiceTypesQuery: IRequest<IEnumerable<IdValue<int>>>
{
    public class GetServiceTypesQueryHandler : IRequestHandler<GetServiceTypesQuery,IEnumerable<IdValue<int>>>
    {
        private readonly IApplicationDbContext _context;

        public GetServiceTypesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<IdValue<int>>> Handle(GetServiceTypesQuery request, CancellationToken cancellationToken)
        {
            return await _context.ServiceTypes
                .Select(c => new IdValue<int>()
                {
                    Id = c.Id,
                    Value = c.Label
                }).AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
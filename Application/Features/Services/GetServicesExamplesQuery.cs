using Application.Common.Models;
using Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Services;

public class GetServiceExamplesQuery: IRequest<IEnumerable<IdValue<int>>>
{
    public class GetServiceExamplesQueryHandler : IRequestHandler<GetServiceExamplesQuery,IEnumerable<IdValue<int>>>
    {
        private readonly IApplicationDbContext _context;

        public GetServiceExamplesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<IdValue<int>>> Handle(GetServiceExamplesQuery request, CancellationToken cancellationToken)
        {
            return await _context.ServiceExamples
                .Select(c => new IdValue<int>()
                {
                    Id = c.Id,
                    Value = c.Label
                }).AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
using Application.Common.Models;
using Application.Contracts;
using Application.Validation;
using Application.Wrappers;
using LanguageExt.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Sport.GetSports;

public class GetSportsQuery : IRequest<IEnumerable<IdValue<int>>>
{
    public class GetSportsHandler : IRequestHandler<GetSportsQuery,IEnumerable<IdValue<int>>>
    {
        private readonly IApplicationDbContext _context;

        public GetSportsHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<IdValue<int>>> Handle(GetSportsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Sports
                .Select(c => new IdValue<int>()
                {
                    Id = c.Id,
                    Value = c.Name
                }).AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
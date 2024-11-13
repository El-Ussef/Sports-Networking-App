using Application.Common.Options;
using Application.Contracts;
using Application.Features.Athletes.GetAthletes;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Features.Profile.GetProfilesByCriteria;

public class GetProfilesByCriteriaQuery : IRequest<PagedResponse<IEnumerable<ProfilesResponse>>>
{
    public FilterCriteria Filter { get; set; }
    
    public class GetProfilesByCriteriaQueryHandler : IRequestHandler<GetProfilesByCriteriaQuery,PagedResponse<IEnumerable<ProfilesResponse>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IOptions<StorageOption> _storageOptions;

        public GetProfilesByCriteriaQueryHandler(
            IApplicationDbContext context,
            IOptions<StorageOption> storageOptions)
        {
            _context = context;
            _storageOptions = storageOptions;
        }
        public async Task<PagedResponse<IEnumerable<ProfilesResponse>>> Handle(GetProfilesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;
            var query = _context.AppUsers.AsQueryable();
            
            //!string.IsNullOrEmpty(filter.Type) && Enum.TryParse<UserType>(filter.Type, true, out var userTypeEnum)
            if (filter.SpecialityId is > 0)
            {
                query = query.Where(u => u.SpecialityId == filter.SpecialityId);
            }
            
            if (filter.SportId is > 0)
            {
                query = query.Where(u => u.SportId == filter.SportId);
            }
            
            if (filter.CityId is > 0)
            {
                query = query.Where(u => u.CityId == filter.CityId);
            }
            
            var result = await query.Select(u => new ProfilesResponse
            {
                Id = u.Id,
                Ref = u.RefId.ToString(),
                FullName = string.Join(" ",u.FirstName, u.LastName, u.ThirdName),
                City = u.City.Label,
                //Sport = u.SportId.HasValue ? u.Sport.Name : string.Empty,
                Photo = u.ProfilePicturePath,
                Speciality = u.Speciality != null ? u.Speciality.Label.ToString() : u.UserType.ToString()
            }).ToPagedListAsync(filter.PageNumber, filter.PageSize);
            
            return new PagedResponse<IEnumerable<ProfilesResponse>>(result, filter.PageNumber, filter.PageSize);

        }
    }
}
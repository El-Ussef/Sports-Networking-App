using Application.Contracts;
using Application.Features.Profile;
using Application.Validation;
using Application.Wrappers;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Account;

public class GetProfileAccountByIdQuery : IRequest<Result<ProfileAccountDto, ValidationFailed>>
{
    public string Type { get; set; }

    public string RefId { get; set; }

    public class
        GetProfileAccountByIdQueryHandler : IRequestHandler<GetProfileAccountByIdQuery, Result<ProfileAccountDto, ValidationFailed>>
    {
        private readonly IApplicationDbContext _context;

        public GetProfileAccountByIdQueryHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ProfileAccountDto, ValidationFailed>> Handle(GetProfileAccountByIdQuery request,
            CancellationToken cancellationToken)
        {

            if (!Enum.TryParse<UserType>(request.Type, true, out var userTypeEnum))
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user type");
                return new Result<ProfileAccountDto, ValidationFailed>(new ValidationFailed(failure));
            }
            Guid userId = Guid.Parse(request.RefId);
            var user = await _context.AppUsers
                .Include(a => a.Speciality)
                .Where(u => u.UserType == userTypeEnum && u.RefId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                var failure = new ValidationFailure("GetProfileByIdQuery", "Failed to Find user");
                return new Result<ProfileAccountDto, ValidationFailed>(new ValidationFailed(failure));
            }

            ProfileAccountDto result = new ProfileAccountDto()
            {
                CityId = user.CityId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ThirdName = user.ThirdName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                SpecialityId = user.SpecialityId,
                SportId = user.SportId,
                NationalityId = user.NationalityId,
                ProfilePicturePath = user.ProfilePicturePath,
                CoverImagePath = user.CoverImagePath,
                Ref = user.RefId,
                JobTitle = user.JobTitle,
                Birthdate = user.Birthdate,
                Presentation = user.Presentation,
                Instagram = user.InstagramLink,
                Facebook = user.FacebookLink,
                LinkedIn = user.LinkedInLink,
                Youtube = user.YoutubeLink,
                Height = user.Height,
                Weight = user.Weight,
                CareerStart = user.CareerStart,
                
            };

            return result;
        }
    }
}
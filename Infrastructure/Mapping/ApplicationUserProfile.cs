using AutoMapper;
using Domain.Entities;
using Infrastructure.Identity.Entities;

namespace Infrastructure.Mapping;

public class ApplicationUserProfile : Profile
{
    public ApplicationUserProfile()
    {
        CreateMap<AppUser, ApplicationUser>();
        CreateMap<ApplicationUser, AppUser>();

    }
}
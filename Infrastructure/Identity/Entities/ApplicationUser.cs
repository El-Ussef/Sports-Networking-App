using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entities;

public class ApplicationUser : IdentityUser,IMapFrom<AppUser>,IMapTo<AppUser>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    //public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    //public string LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public bool? IsDeleted { get; set; }
    public string Email { get; set; } = string.Empty;
    public int DomainId { get; set; }
    public UserType UserType { get; set; }
    public DateTime DateCreated { get; set; }
    
    public int CityId { get; set; }
    public City City { get; set; }
    
    public int? SportId { get; set; }
    public Sport? Sport { get; set; }
    
    public int? SpecialityId { get; set; }
    public Speciality? Speciality { get; set; } //Athletes,Medical and Health,Coach
    
    public string? JobTitle { get; set; } = string.Empty;
    
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
    
    public virtual void Mapping(Profile profile)
    {
        profile.CreateMap<AppUser,ApplicationUser>();
    }
}
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entities;

public class ApplicationUser : IdentityUser,IMapFrom<User>,IMapTo<User>
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = string.Empty;
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public bool? IsDeleted { get; set; }
    public UserType UserType { get; set; }
    
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
}
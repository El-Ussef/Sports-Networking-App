using Domain.Common;

namespace Domain.Entities;

public class Sport : BaseEntity
{
    public string Name { get; set; }
    public string[]? Hints { get; set; }
    
    public int? SportCategoryId { get; set; }
    public SportCategory? Category { get; set; }

    //public List<SportTranslation> Translations { get; set; }
}
using Domain.Common;

namespace Domain.Entities;

public class SportCategory: BaseEntity
{
    public string Label { get; set; } = string.Empty;

    public string[]? Hints { get; set; }

    public List<Sport> Sports { get; set; }
    //public List<SportCategoryTranslation> Translations { get; set; }

}
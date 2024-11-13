using Domain.Common;

namespace Domain.Entities;

public class SportCategoryTranslation : BaseEntity
{
    public string CodeLang { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;

    public int SportCategoryId { get; set; }
    public SportCategory Category { get; set; }
}
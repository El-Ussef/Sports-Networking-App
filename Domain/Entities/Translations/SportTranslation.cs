using Domain.Common;

namespace Domain.Entities;

public class SportTranslation : BaseEntity
{
    public string CodeLang { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;

    public int SportId { get; set; }
    public Sport Sport { get; set; }

}
using Domain.Common;

namespace Domain.Entities;

public class ServiceTypeTranslation : BaseEntity
{
    public string CodeLang { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;

    public int? ServiceTypeId { get; set; }
    public ServiceType ServiceType { get; set; }
}
using Domain.Common;

namespace Domain.Entities;

public class City : BaseEntity
{
    public string Label { get; set; } = string.Empty;

    public int? RegionId { get; set; }
    public Region? Region { get; set; }

    public int? ProvinceId { get; set; }
    public Province? Province { get; set; }

    public int CountryId { get; set; }
    public Country Country { get; set; }
}
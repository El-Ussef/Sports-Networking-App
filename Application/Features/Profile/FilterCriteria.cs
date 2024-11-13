using Application.Wrappers;

namespace Application.Features.Profile;

public class FilterCriteria : PagingParameter
{
    public int? SpecialityId { get; set; }
    public string? Type { get; set; }

    public int? SportId { get; set; }

    public int? CityId { get; set; }
}
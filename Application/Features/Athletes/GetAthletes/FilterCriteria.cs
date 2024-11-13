using Application.Wrappers;

namespace Application.Features.Athletes.GetAthletes;

public class FilterCriteria : PagingParameter
{
    public int? SportId { get; set; }

    public int? CityId { get; set; }

    public int? Age { get; set; }
}
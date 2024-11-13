using Application.Wrappers;

namespace Application.Features.Events;

public class FilterCriteria : PagingParameter
{
    public int? CityId { get; set; }
}
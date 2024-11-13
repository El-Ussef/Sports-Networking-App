using Application.Features.City;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

public class CitiesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCitiesByCountyId(int countryId)
    {
        var result = await Mediator.Send(new GetCitiesByCountryIdQuery{ CountryId = countryId});
        return Ok(result);
    }
}
using Application.Features.Country;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

public class CountriesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetCounties()
    {
        var result = await Mediator.Send(new GetCountriesQuery {});
        return Ok(result);
    }

    // public async Task<IActionResult> InsertAllCountries(List<Country> countries)
    // {
    //     
    // }
}
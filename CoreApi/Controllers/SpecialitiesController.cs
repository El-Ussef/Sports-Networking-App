using Application.Common.Models;
using Application.Features.Specialities;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

public class SpecialitiesController : BaseApiController
{
    // GET
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IdValue<int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSpeciality()
    {
        var result = await Mediator.Send(new GetSpecialitiesQuery {});
        
        if (result.Any())
        {
            return Ok(result);
        }

        return BadRequest();
    }
}
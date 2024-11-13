using Application.Common.Models;
using Application.Features.Sport.GetSports;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

public class SportsController : BaseApiController
{

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IdValue<int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSports()
    {
        return Ok(await Mediator.Send(new GetSportsQuery()));
    }
    

    [HttpGet("filtered")]
    [ProducesResponseType(typeof(IEnumerable<IdValue<int>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSportsByCriteria([FromQuery] SportsCriteria criteria)
    {
        return Ok(await Mediator.Send(new GetSportsByCriteriaQuery()
        {
            Criteria = criteria
        }));
    }
}
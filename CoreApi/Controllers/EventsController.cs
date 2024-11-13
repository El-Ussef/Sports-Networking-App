using Application.Features.Achievements;
using Application.Features.Events;
using Application.Mappings;
using CoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

public class EventsController: BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto data)
    {
        var result = await Mediator.Send(new CreateEventCommand() { NewEvent = data});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateEvent(int id,[FromBody] AchievementDto data)
    {
        var result = await Mediator.Send(new UpdateAchievementCommand() { Data = data, Id = id});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }

    [HttpGet]
    public async Task<IActionResult> GetEventsByCriteria([FromBody] FilterCriteria criteria)
    {
        var result = await Mediator.Send(new GetEventsByCriteriaQuery { Filter = criteria});
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(int id)
    {
        var result = await Mediator.Send(new GetEventQuery { Id = id});
        return Ok(result);
    }
    
}
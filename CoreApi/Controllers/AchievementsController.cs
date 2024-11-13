using Application.Features.Achievements;
using Application.Features.Athletes;
using Application.Features.Profile;
using Application.Features.Profile.UpdateProfile;
using Application.Mappings;
using Microsoft.AspNetCore.Mvc;
using AchievementAthleteDto = Application.Features.Athletes.AchievementAthleteDto;

namespace CoreApi.Controllers;

//TODO: injuect current user whenever userId
public class AchievementsController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateAchievement([FromBody] AchievementDto data)
    {
        var result = await Mediator.Send(new CreateAchievementCommand() { Data = data});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateAchievement(int id,[FromBody] AchievementDto data)
    {
        var result = await Mediator.Send(new UpdateAchievementCommand() { Data = data, Id = id});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteAchievement(int id,Guid userId)
    {
        var result = await Mediator.Send(new DeleteAchievementCommand() { Id = id, RefId = userId});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAchievementsByUserId(Guid id)
    {
        var result = await Mediator.Send(new GetAchievementsByUserIdQuery() { UserId = id});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAchievementByUserId(int id, Guid userId)
    {
        var result = await Mediator.Send(new GetAchievementByUserIdQuery() { UserId = userId, Id = id});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    //TODO:delete
}
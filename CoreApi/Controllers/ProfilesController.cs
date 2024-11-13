using Application.Common.Models;
using Application.Features.Account;
using Application.Features.Profile;
using Application.Features.Profile.GetProfileById;
using Application.Features.Profile.GetProfilesByCriteria;
using Application.Features.Profile.UpdateProfile;
using Application.Mappings;
using Application.Validation;
using Application.Wrappers;
using CoreApi.Models;
using Domain.Enums;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

public class ProfilesController: BaseApiController
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ProfileAccountDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfiles([FromQuery] FilterCriteria filter)
    {
        var result = await Mediator.Send(new GetProfilesByCriteriaQuery { Filter = filter});
        return Ok(result);
    }
    //[FromHeader(Name = "X-Type")] string type
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">profile refId </param>
    /// <param name="type">userType as string</param>
    /// <returns></returns>
    [HttpGet("{type}/{id}")]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailed), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProfile([FromRoute] string type, Guid id)
    {
        var result = await Mediator.Send(new GetProfileByIdQuery { Type = type, RefId = id});
        return result.Match<IActionResult>(user =>
        {
            return Ok(user);
        }, exception => BadRequest(exception.MapToResponse()));
    }
}
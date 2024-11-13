using Application.Features.Profile;
using Application.Features.Services;
using Application.Mappings;
using Application.Validation;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

public class ServicesController : BaseApiController
{
    [HttpGet]//Result<List<ServiceResponse>,ValidationFailed>
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailed), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetServicesByUserId(Guid userId)
    {
        var result = await Mediator.Send(new GetServicesByUserIdQuery { UserId = userId });
        return result.Match<IActionResult>(service =>
        {
            return Ok(service);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailed), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateService([FromBody] ServiceDto data)
    {
        var result = await Mediator.Send(new CreateServiceCommand{ CustomService = data });
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailed), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateService(int id,[FromBody] ServiceDto data)
    {
        var result = await Mediator.Send(new UpdateServiceCommand() { Data = data});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpDelete]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailed), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteService(int id,Guid userId)
    {
        var result = await Mediator.Send(new DeleteServiceCommand() { Id = id, RefId = userId});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpPut("save")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailed), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SaveAll(int id,[FromBody] ServiceDto data)
    {
        var result = await Mediator.Send(new UpdateServiceCommand() { Data = data});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpGet("suggestions")]//Result<List<ServiceResponse>,ValidationFailed>
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailed), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetServicesExamples()
    {
        var result = await Mediator.Send(new GetServiceExamplesQuery { });
        return Ok(result);

    }
    
    [HttpGet("type")]//Result<List<ServiceResponse>,ValidationFailed>
    [ProducesResponseType(typeof(List<ServiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailed), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetServicesTypes()
    {
        var result = await Mediator.Send(new GetServiceTypesQuery { });
        return Ok(result);

    }
    
}
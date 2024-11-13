
using Application.Common.Models;
using Application.Contracts;
using Application.Features.Locations;
using Application.Mappings;
using CoreApi.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

[Authorize]
public class LocationsController: BaseApiController
{
    private readonly ICurrentUserService _currentUserService;

    public LocationsController(
        ICurrentUserService currentUserService

        )
    {
        _currentUserService = currentUserService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromForm] LocationDto data)
    {

        Location location = new Location()
        {
            LocationName = data.LocationName,
            Address = data.Address,
            Latitude = data.Latitude,
            Longitude = data.Longitude,
            OpeningHours = new OpeningHours
            {
                Start = TimeSpan.Parse(data.OpeningHoursStart),
                End = TimeSpan.Parse(data.OpeningHoursEnd)
            },
            CreatedTime = DateTime.UtcNow,
            AppUserId = int.Parse(_currentUserService.Id)
        };
        
        List<FileUpload> locationImages = new List<FileUpload>();
        
        if (data.LocationImages is { Count: > 0 })
        {
            foreach (var image in data.LocationImages)
            {
                var imageUrl = new FileUpload
                {
                    FileName = image.FileName,
                    Data = image.OpenReadStream()
                };
                locationImages.Add(imageUrl);
            }
        }
        var result = await Mediator.Send(new CreateLocationCommand() { Data = location,LocationImages = locationImages});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    // GET: api/locations
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LocationVm), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocationById(int id)
    {
        var result = await Mediator.Send(new GetLocationByIdQuery{ Id = id });
        return result.Match<IActionResult>(b =>
        {
            return Ok(b);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    // GET: api/locations
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LocationVm>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations()
    {
        var result = await Mediator.Send(new GetLocationsQuery{});
        return result.Match<IActionResult>(b =>
        {
            return Ok(b);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAchievement(int id,[FromForm] LocationUpdateVm data)
    {
        var result = await Mediator.Send(new UpdateLocationCommand() { LocationUpdateVm = data, Id = id});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAchievement(int id)
    {
        var result = await Mediator.Send(new DeleteLocationCommand { Id = id});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
}

/*
 *
 * dotnet ef migrations add AddLocationWithOpeningHours
dotnet ef database update
 */
using Application.Common.Models;
using Application.Contracts;
using Application.Features.Account;
using Application.Features.Athletes.UpdateProfile;
using Application.Features.Auth.login;
using Application.Features.Profile;
using Application.Features.Profile.DeleteCommands;
using Application.Features.Profile.GetProfileById;
using Application.Features.Profile.UpdateProfile;
using Application.Features.Registration;
using Application.Mappings;
using CoreApi.Models;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhysicalInformationDto = Application.Features.Profile.PhysicalInformationDto;

namespace CoreApi.Controllers;

/// <summary>
/// Endpoint for register a user (password should at least have 8 characters)
/// </summary>
[Authorize]
public class AccountsController : BaseApiController
{
    private readonly ICurrentUserService _currentUserService;

    public AccountsController(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProfileInformation()
    {
        var user = _currentUserService;
        var result = await Mediator.Send(new GetProfileAccountByIdQuery{ RefId=user.UserId, Type = user.UserType });
        return result.Match<IActionResult>(b =>
        {
            return Ok(b);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    
    [HttpGet("images")]
    public async Task<IActionResult> GetImages()
    {
        var user = _currentUserService;
        var result = await Mediator.Send(new GetProfileImagesQuery{ RefId=user.UserId, Type = user.UserType });
        return result.Match<IActionResult>(b =>
        {
            return Ok(b);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    // [HttpPost("UpdateProfile/{id}")]
    // public async Task<IActionResult> UpdateProfile(Guid id ,[FromBody] ProfileDto request)
    // {
    //     var result = await Mediator.Send(new UpdateAthleteProfileCommand{ Id=id, Data = request });
    //     return result.Match<IActionResult>(b =>
    //     {
    //         return Ok(b);
    //     }, exception => BadRequest(exception.MapToResponse()));
    //
    // }
    
    [HttpPut("social-media")]
    public async Task<IActionResult> UpdateSocialMedia(Guid id,[FromBody] SocialMediaDto data)
    {
        var user = _currentUserService;

        var result = await Mediator.Send(new UpdateSocialMediaCommand { Data = data, RefId = user.UserId});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpGet("social-media")]
    public async Task<IActionResult> GetSocialMedia()
    {
        var user = _currentUserService;
        var result = await Mediator.Send(new GetSocialMediaQuery { RefId = user.UserId,Type = user.UserType});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpPut("measurement")]
    public async Task<IActionResult> UpdatePhysicalInformation(Guid id,[FromBody] PhysicalInformationDto data)
    {
        var user = _currentUserService;

        var result = await Mediator.Send(new UpdatePhysicalInformationCommand { Data = data, RefId = user.UserId});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpGet("measurement")]
    public async Task<IActionResult> GetPhysicalInformation()
    {
        var user = _currentUserService;
        var result = await Mediator.Send(new GetPhysicalInformationQuery { RefId = user.UserId,Type = user.UserType});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    [HttpPut("images")]
    public async Task<IActionResult> UpdateImageGallery(Guid id,[FromForm] ImageDto data)
    {
        var user = _currentUserService;
        var coverImage = data.CoverImage != null
            ? new FileUpload
            {
                FileName = data.CoverImage.FileName,
                Data = data.CoverImage.OpenReadStream()
            }
            : null;

        var profileImage = data.ProfilePhoto != null
            ? new FileUpload
            {
                FileName = data.ProfilePhoto.FileName,
                Data = data.ProfilePhoto.OpenReadStream()
            }
            : null;
        
        var photoOne = data.PhotoOne != null
            ? new FileUpload
            {
                FileName = data.PhotoOne.FileName,
                Data = data.PhotoOne.OpenReadStream()
            }
            : null;
        
        var photoTwo = data.PhotoTwo != null
            ? new FileUpload
            {
                FileName = data.PhotoTwo.FileName,
                Data = data.PhotoTwo.OpenReadStream()
            }
            : null;
        
        ImageGalleryDto request = new ImageGalleryDto
        {
            ProfilePicture = profileImage,
            CoverImage = coverImage,
            PhotoOne = photoOne,
            PhotoTwo = photoTwo
        };
        
        var result = await Mediator.Send(new UpdateImageGalleryCommand { Data = request, RefId = user.UserId});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }
    
    
    [HttpPut]
    public async Task<IActionResult> UpdateProfile(Guid id,[FromForm] UserUpdateDto data)
    {
        var coverImage = data.CoverImage != null
            ? new FileUpload
            {
                FileName = data.CoverImage.FileName,
                Data = data.CoverImage.OpenReadStream()
            }
            : null;

        var profileImage = data.ProfilePicture != null
            ? new FileUpload
            {
                FileName = data.ProfilePicture.FileName,
                Data = data.ProfilePicture.OpenReadStream()
            }
            : null;
        
        ProfileRequestDto profile = new ProfileRequestDto
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            ThirdName = data.ThirdName,
            Birthdate = data.Birthdate,
            CityId = data.CityId,
            Email = data.Email,
            CareerStart = data.CareerStart,
            OrganisationName = data.OrganisationName,
            PhoneNumber = data.PhoneNumber,
            JobTitle = data.JobTitle,
            Presentation = data.Presentation,
            SportId = data.SportId,
            NationalityId = data.NationalityId,
            SpecialityId = data.SpecialityId,
            CoverImage = coverImage,
            ProfilePicture = profileImage
        };
        
        var result = await Mediator.Send(new UpdateProfileCommand { Data = profile, RefId = id});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse()));
    }

    [HttpDelete("images")]
    public async Task<IActionResult> DeleteImages(ImageDeleteDto request)
    {
        var user = _currentUserService;
        
        var result = await Mediator.Send(new DeleteImageCommand(){ Data = request,RefId = user.UserId});
        return result.Match<IActionResult>(ok =>
        {
            return Ok(ok);
        }, exception => BadRequest(exception.MapToResponse())); 
    }

    [HttpGet("current-user")]
    public IActionResult GetCurrentUser()
    {
        var isAuth = _currentUserService;
        return Ok(new
        {
            UserId = _currentUserService.UserId,
            UserName = _currentUserService.UserName,
            UserType = _currentUserService.UserType,
            Id = _currentUserService.Id
        });
    }
    
    [Authorize]
    [HttpGet("isAuth")]
    public IActionResult CheckAuth()
    {
        var isAuth = _currentUserService;
        return Ok(new
        {
            UserId = _currentUserService.UserId,
            UserName = _currentUserService.UserName,
            UserType = _currentUserService.UserType,
            Id = _currentUserService.Id
        });
    }
    
}
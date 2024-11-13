using Application.Common.Models;
using Application.Contracts;
using Application.Features.Auth.login;
using Application.Features.Registration;
using Application.Mappings;
using CoreApi.Controllers;
using CoreApi.Models;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public class AuthController : BaseApiController
{
    private readonly IJwtTokenService _jwtTokenGenerator;
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    public AuthController(
        IJwtTokenService jwtTokenGenerator,
        IIdentityService identityService,
        IEmailService emailService
        )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _identityService = identityService;
        _emailService = emailService;
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromForm] UserRegistrationDto data)
    {
        var profilePhoto = data.ProfilePhoto != null
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

        var request = new RegistrationRequest()
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            //ThirdName = data.ThirdName,
            // Height = data.Height,
            // Weight = data.Weight,
            CareerStart = data.CareerStart,
            PhoneNumber = data.PhoneNumber,
            OrganisationName = data.OrganisationName,
            CityId = data.CityId,
            SportId = data.SportId,
            Email = data.Email,
            Password = data.Password,
            JobTitle = data.JobTitle,
            SpecialityId = data.SpecialityId,
            Birthdate = data.Birthdate,
            ProfilePhoto = profilePhoto,
            PhotoTwo = photoTwo,
            PhotoOne = photoOne,
            Gender = data.Gender,
            Experience = data.Experience
            
        };

        var result =  await _identityService.RegisterAppUserAsync(request, data.Password);
        
        return result.Match<IActionResult>(b => { return Ok(b); },
            ex => BadRequest(ex.MapToResponse()));
    }

    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto credentials)
    {
        // TODO: put in service
        var result = await Mediator.Send(new LoginQuery { Login = credentials.Username, Password = credentials.Password });
        return result.Match<IActionResult>(b => { return Ok(b); },
            ex => BadRequest(ex.MapToResponse()));
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
    {
        if (tokenDto is null)
        {
            return BadRequest("Invalid client request");
        }

        var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(tokenDto.Token);
        if (principal == null)
        {
            return BadRequest("Invalid token");
        }

        var userName = principal.Identity.Name;
        var user = await _identityService.GetUserByEmailAsync(userName);

        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid client request");
        }

        var newToken = _jwtTokenGenerator.GenerateJwtToken(user);
        var newRefreshToken = _jwtTokenGenerator.GenerateJwtRefreshToken(user);

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _identityService.UpdateAsync(user);

        return Ok(new 
        { 
            token = newToken, 
            refreshToken = newRefreshToken 
        });
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        var user = await _identityService.GetUserByEmailAsync(forgotPasswordDto.Email);
        if (user == null) return BadRequest("Invalid Email");

        var token = await _identityService.GeneratePasswordResetTokenAsync(user);
        
        var resetLink = Url.Action(nameof(ForgotPassword), "Auth", new { token, email = user.Email }, Request.Scheme);

        // Send resetLink via email
        await _emailService.SendEmailAsync(user.Email, "Reset your password", $"Please reset your password by clicking this link: {resetLink}");

        return Ok("Password reset link has been sent to your email.");
    }

    
}
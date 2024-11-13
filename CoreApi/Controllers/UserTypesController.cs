using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers;

[AllowAnonymous]
public class UserTypesController : BaseApiController
{
    // GET
    [HttpGet]
    public IActionResult GetUserTypes()
    {
        var result = Enum.GetValuesAsUnderlyingType<UserType>().ToString();
        List<string> colorNames = new List<string>(Enum.GetNames(typeof(UserType)));
        return Ok(colorNames.Skip(1).ToList());
    }
}
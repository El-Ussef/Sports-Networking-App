using Application.Validation;
using Application.Wrappers;

namespace Application.Mappings;

public static class RegistrationMappings
{
    public static ErrorResponse MapToResponse(this ValidationFailed failed)
    {
        return new ErrorResponse
        {
            errors = failed.Errors.Select(x => new ErrorDtoReponse
            {
                ErrorMessage = x.ErrorMessage,
                Source = x.Source
            })
        };
    }
}
namespace Application.Wrappers;

public class ErrorResponse
{
    public required IEnumerable<ErrorDtoReponse> errors { get; init; }
}

public class ErrorDtoReponse
{
    public string Source { get; init; }
    public string ErrorMessage { get; init; }
}
namespace Application.Validation;

public class ValidationFailure
{
    public ValidationFailure(string source, string errorMessage)
    {
        Source = source;
        ErrorMessage = errorMessage;
    }

    public string Source { get; }
    public string ErrorMessage { get; set; }
}
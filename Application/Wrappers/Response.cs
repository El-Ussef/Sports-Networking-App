namespace Application.Wrappers;

public class Response<T>
{

    public Response(){}

    public Response(T data, string? message = null)
    {
        Succeeded = data == null ? false : true;
        Message = message ?? string.Empty;
        Data = data;
    }
    public Response(string message)
    {
        Succeeded = false;
        Message = message;
    }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}

namespace Application.Wrappers;

public class PagingParameter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    
    public PagingParameter()
    {
        PageNumber = 1;
        PageSize = 10;
    }
    public PagingParameter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 10 ? pageSize: 10;
    }
}
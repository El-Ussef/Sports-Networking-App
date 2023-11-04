namespace Domain.Entities;

public class Province
{
    public int Id { get; set; }

    public string Label { get; set; }

    public Region Region { get; set; }
}
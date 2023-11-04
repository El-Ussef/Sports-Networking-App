namespace Domain.Entities;

public class Region
{
    public int Id { get; set; }

    public string Label { get; set; }

    public Country Country { get; set; }
}
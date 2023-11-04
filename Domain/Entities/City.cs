namespace Domain.Entities;

public class City
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;

    public Region? Region { get; set; }

    public Province? Province { get; set; }
}
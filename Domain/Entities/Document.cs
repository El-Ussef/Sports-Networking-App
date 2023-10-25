using Domain.Enums;

namespace Domain.Entities;

public class Document
{
    public int Id { get; set; }
    public int AthleteId { get; set; }
    public Athlete Athlete { get; set; }
    public string FilePath { get; set; }
    public FileType DocumentType { get; set; }
    public DateTime UploadDate { get; set; }
}
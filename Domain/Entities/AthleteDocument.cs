using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;


public class Document : BaseEntity
{
    public int AthleteId { get; set; }
    public Athlete Athlete { get; set; }
    public string FilePath { get; set; }
    public FileType DocumentType { get; set; }
    public DateTime UploadDate { get; set; }
}
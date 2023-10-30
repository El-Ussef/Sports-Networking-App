using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Video : BaseEntity
{
    public int AthleteId { get; set; }
    public Athlete Athlete { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FilePath { get; set; }
    public string ThumbnailPath { get; set; }
    public DateTime UploadDate { get; set; }
    public GeneralStatus Status { get; set; }
    public List<VideoReport> VideoReports { get; set; }
}
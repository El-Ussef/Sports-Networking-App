using Domain.Common;

namespace Domain.Entities;

public class VideoReport : BaseEntity
{
    public int VideoId { get; set; }
    public Video Video { get; set; }
    public string Reason { get; set; }
    public DateTime ReportDate { get; set; }
}
using Domain.Enums;

namespace Domain.Common;

public class AbstractDocument : BaseEntity<Guid>
{
    public string Uri { get; set; }
    public byte[] Data { get; set; }
    public string MimeType { get; set; }
    public bool? IsGenerated { get; set; }
    public DateTime UploadDate { get; set; }

    public FileType Type { get; set; }
}
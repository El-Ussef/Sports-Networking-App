using System.Text.Json.Serialization;

namespace Application.Features.Profile;

public class ImageDeleteDto
{
    public List<ImageField> ImageFields { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ImageField {
    ProfilePhoto = 0,
    PhotoOne = 1,
    PhotoTwo = 2,
    CoverImage = 3
}
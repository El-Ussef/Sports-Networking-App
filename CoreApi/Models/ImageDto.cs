namespace CoreApi.Models;

public class ImageDto
{
    public IFormFile? CoverImage { get; set; }
    
    public IFormFile? ProfilePhoto { get; set; }
    
    public IFormFile? PhotoOne { get; set; } 
    
    public IFormFile? PhotoTwo { get; set; }
}
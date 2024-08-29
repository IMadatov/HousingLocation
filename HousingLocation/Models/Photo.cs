namespace HousingLocation.Models;

public class Photo
{
    public int PhotoId { get; set; }

    public required string FileName { get; set; }
    
    public required string OriginalName { get; set; }
}

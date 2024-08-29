namespace HousingLocation.Dto
{
    public class PhotoDto
    {
        public int PhotoId { get; set; }

        public required string FileName { get; set; }

        public required string OriginalName { get; set; }
    }
}

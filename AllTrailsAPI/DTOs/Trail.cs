namespace AllTrailsAPI.DTOs
{
    public class Trail
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Length { get; set; }
        public int ElevationGain { get; set; }
        public string Location { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }
    }
}

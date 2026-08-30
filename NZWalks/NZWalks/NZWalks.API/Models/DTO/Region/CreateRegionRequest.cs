namespace NZWalks.API.Models.DTO.Region
{
    public class CreateRegionRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}

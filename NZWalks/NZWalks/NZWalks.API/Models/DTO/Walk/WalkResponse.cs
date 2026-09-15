using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Difficulty;
using NZWalks.API.Models.DTO.Region;

namespace NZWalks.API.Models.DTO.Walk
{
    public class WalkResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public Guid RegionId { get; set; }

        public Guid DifficultyId { get; set; }

        public RegionResponse region { get; set; }

        public DifficultyResponse difficulty { get; set;  }

    }
}

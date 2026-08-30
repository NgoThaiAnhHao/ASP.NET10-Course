using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Region;

namespace NZWalks.API.Mapper
{
    public class RegionMapper
    {
        public static RegionResponse ToResponse(Region region)
        {
            return new RegionResponse
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            };
        }

        public static Region ToEntity(CreateRegionRequest createRegionRequest)
        {
            return new Region
            {
                Code = createRegionRequest.Code,
                Name = createRegionRequest.Name,
                RegionImageUrl = createRegionRequest.RegionImageUrl
            };
        }

    }
}

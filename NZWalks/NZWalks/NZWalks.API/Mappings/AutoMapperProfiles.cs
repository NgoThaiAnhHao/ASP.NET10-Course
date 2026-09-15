using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Difficulty;
using NZWalks.API.Models.DTO.Region;
using NZWalks.API.Models.DTO.Walk;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile {

        public AutoMapperProfiles()
        {
            // Map Region -> RegionResponse, RegionResponse -> Region
            CreateMap<Region, RegionResponse>().ReverseMap();

            // Map Create Region Request -> Region
            CreateMap<CreateRegionRequest, Region>();

            // Map Update Region Request -> Region
            CreateMap<UpdateRegionRequest, Region>();

            // Map Create Walk Request -> Walk
            CreateMap<CreateWalkRequest, Walk>();

            // Map Walk -> Walk Response
            CreateMap<Walk, WalkResponse>();

            // Map Difficulty -> Difficulty Response
            CreateMap<Difficulty, DifficultyResponse>();

            // Map Uodate Walk Request => Walk
            CreateMap<UpdateWalkRequest, Walk>();
        }
    }
}

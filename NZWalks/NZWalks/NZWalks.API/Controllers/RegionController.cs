using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Mapper;
using NZWalks.API.Models.DTO.Region;

namespace NZWalks.API.Controllers
{
    [Route("api/regions")]
    [ApiController]
    public class RegionController : ControllerBase
    {

        private readonly NZWalksDbContext _dbContext;

        public RegionController(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        // GET ALL REGIONS
        // GET https://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Region> regions = _dbContext.Regions.ToList();

            // Mapping to dto and return
            return Ok(
                regions
                .Select(region => RegionMapper.ToResponse(region))
                .ToList()
            );
        }

        // GET REGIONS BY ID
        // GET https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            // Dùng Find (Find chỉ tìm dựa vào Primary Key)
            Region? region = _dbContext.Regions.Find(id);
            // Region? region = _dbContext.Regions.FirstOrDefault(x => x.Code == code)

            // Check region null
            if (region == null)
            {
                return NotFound();
            }

            return Ok(
                RegionMapper.ToResponse(region) 
            );
        }

        // POST CREATE NEW REGION
        // POST https://localhost:portnumber/regions
        [HttpPost]
        public IActionResult Create([FromBody] CreateRegionRequest createRegionRequest) {
            // Map to entity
            Region? region = RegionMapper.ToEntity(createRegionRequest);

            // Save to db
            _dbContext.Regions.Add(region);
            _dbContext.SaveChanges();

            // Map to response
            RegionResponse regionResponse = RegionMapper.ToResponse(region);

            return CreatedAtAction(
                // tạo Location trong Response Headers.
                nameof(GetById), 
                new { id = region.Id},

                // Response Body
                regionResponse);
        }

        // UPDATE REGION
        // PUT https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromBody] UpdateRegionRequest updateRegionRequest,
                                    [FromRoute] Guid id)
        {
            // Check if region exists
            Region? regionFound = _dbContext.Regions.Find(id);
            if (regionFound == null) {
                return NotFound();
            }


            // Map to entity and update
            regionFound.Code = updateRegionRequest.Code;
            regionFound.Name = updateRegionRequest.Name;
            regionFound.RegionImageUrl = updateRegionRequest.RegionImageUrl;

            // Save to database
            _dbContext.SaveChanges();

            return Ok( 
                RegionMapper.ToResponse(regionFound) 
            );
        }

        // DELETE REGION
        // DELETE https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            // Check if region exists
            Region? regionFound = _dbContext.Regions.Find(id);

            if (regionFound == null) {
                return NotFound();
            }

            // Delete region
            _dbContext.Regions.Remove(regionFound);
            _dbContext.SaveChanges();

            return Ok(
                RegionMapper.ToResponse(regionFound)
            );
        }
    }
}

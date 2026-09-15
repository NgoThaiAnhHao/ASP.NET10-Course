using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Region;
using NZWalks.API.Repository;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/regions")]
    [ApiController]
    public class RegionController : ControllerBase
    {

        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionController> _logger;

        public RegionController(
            NZWalksDbContext dbContext, 
            IRegionRepository regionRepository, 
            IMapper mapper,
            ILogger<RegionController> logger)
        {
            this._dbContext = dbContext;
            this._regionRepository = regionRepository;
            this._mapper = mapper;
            this._logger = logger;
        }

        // GET ALL REGIONS
        // GET https://localhost:portnumber/api/regions
        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Get All method was invoked");
            _logger.LogWarning("This is warning log");
            _logger.LogError("This is error log");

            var regions = await _regionRepository.GetAllAsync();

            _logger.LogInformation($"Finished Get All method request with data: " +
                $"{JsonSerializer.Serialize(regions)}"
            );

            // Mapping to dto and return
            // _mapper.Map<Kiểu dữ liệu trả về>(Kiểu dữ liệu ban đầu);
            return Ok(
               _mapper
               .Map<List<RegionResponse>>(regions)
            );
        }

        // GET REGIONS BY ID
        // GET https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Region? region = await _regionRepository.GetById(id);

            // Check region null
            if (region == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper
                .Map<RegionResponse>(region)
            );
        }

        // POST CREATE NEW REGION
        // POST https://localhost:portnumber/regions
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] CreateRegionRequest createRegionRequest) {

           
            // Map to entity
            Region? region = _mapper.Map<Region>(createRegionRequest);

            // Save to db
            region = await _regionRepository.Create(region);

            return CreatedAtAction(
                // tạo Location trong Response Headers.
                nameof(GetById), 
                new { id = region.Id},

                // Response Body
                _mapper.Map<RegionResponse>(region)
            );
        }

        // UPDATE REGION
        // PUT https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update(
            [FromBody] UpdateRegionRequest updateRegionRequest,
            [FromRoute] Guid id)
        {
            
            // Map to entity and update
            var region = _mapper.Map<Region>(updateRegionRequest);

            // Save to database
            Region? regionUpdated = await _regionRepository.Update(region, id);

            // Check if region exists
            if (regionUpdated == null)
            {
                return NotFound();
            }

            return Ok( 
                _mapper.Map<RegionResponse>(regionUpdated)
            );
        }

        // DELETE REGION
        // DELETE https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer, Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Region? regionDeleted = await _regionRepository.Delete(id);

            if (regionDeleted == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<RegionResponse>(regionDeleted)
            );
        }
    }
}

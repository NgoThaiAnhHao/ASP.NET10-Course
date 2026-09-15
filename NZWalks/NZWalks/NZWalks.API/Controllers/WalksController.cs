using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Region;
using NZWalks.API.Models.DTO.Walk;
using NZWalks.API.Repositories;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    // api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this._mapper = mapper;
            this._walkRepository = walkRepository;
        }

        // GET ALL WALKS
        // GET: https://localhost:portnumber/api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn, 
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool isAscending = true,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 100)
        {
            // isAscending ?? true: isAscending là null thì cho mặc định truyền true
            var regions = await _walkRepository
                .GetAll(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            throw new Exception("This is a walk exception");

            return Ok(
                _mapper.Map<List<WalkResponse>>(regions)
            );
        }

        // GET WALK BY ID
        // GET: https://localhost:portnumber/api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walk = await _walkRepository.GetById(id);

            if (walk == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<WalkResponse>(walk)
            );

        }

        // POST CREATE NEW WALK
        // POST: https://localhost:portnumber/api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreateWalkRequest createWalkRequest)
        {
         
            // Map to entity
            var walk = _mapper.Map<Walk>(createWalkRequest);

            // Save to database
            var walkSaved = await _walkRepository.Create(walk);

            return Ok(
                _mapper.Map<WalkResponse>(walkSaved)    
            );
        }

        // PUT WALK
        // PUT: https://localhost:portnumber/api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update(
            [FromBody] UpdateWalkRequest updateWalkRequest,
            [FromRoute] Guid id)
        {
        
            // Mapping
            var walk = _mapper.Map<Walk>(updateWalkRequest);

            // Save to database
            var walkUpdated = await _walkRepository.Update(walk, id);

            if (walkUpdated == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<WalkResponse>(walkUpdated)
            );
            
        }

        // DELETE WALK
        // DELETE: https://localhost:portnumber/api/walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDeleted = await _walkRepository.Delete(id);

            if (walkDeleted == null)
            {
                return NotFound();
            }

            return Ok(
                _mapper.Map<WalkResponse>(walkDeleted)
            );
        }
    }
}

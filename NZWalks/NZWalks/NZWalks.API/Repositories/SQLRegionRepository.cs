using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Region;
using NZWalks.API.Repository;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetById(Guid id)
        {
            // return await _dbContext.Regions.FirstOrDefault(x => x.Code == code)

            // Dùng Find (Find chỉ tìm dựa vào Primary Key)
            return await _dbContext.Regions.FindAsync(id);
        }

        public async Task<Region> Create(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> Update(Region region, Guid id)
        {
            // Check if region exists
            Region? regionFound = await _dbContext.Regions.FindAsync(id);

            if (regionFound == null)
            {
                return null;
            }

            // Map to entity and update
            regionFound.Code = region.Code;
            regionFound.Name = region.Name;
            regionFound.RegionImageUrl = region.RegionImageUrl;

            // Save to database
            await _dbContext.SaveChangesAsync();

            return regionFound;
        }

        public async Task<Region?> Delete(Guid id)
        {
            // Check if region exists
            Region? regionFound = await _dbContext.Regions.FindAsync(id);

            if (regionFound == null)
            {
                return null;
            }

            // Delete region
            _dbContext.Regions.Remove(regionFound);
            await _dbContext.SaveChangesAsync();

            return regionFound;
        }
    }
}

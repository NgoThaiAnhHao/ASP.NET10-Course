using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {

        private readonly NZWalksDbContext _dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Walk> Create(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            await _dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> Delete(Guid id)
        {
            // Check if walk exists
            var walkFound = await
                _dbContext
                .Walks
                .Include(w => w.Region)
                .Include(w => w.Difficulty)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (walkFound == null)
            {
                return null;
            }

            // Delete region
            _dbContext.Walks.Remove(walkFound);
            await _dbContext.SaveChangesAsync();

            return walkFound;
        }

        public async Task<List<Walk>> GetAll(
            string? filterOn = null, 
            string? filterQuery = null, 
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 100)
        {
            var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                // StringComparison.OrdinalIgnoreCase: So sánh không phân biệt hoa / thường của "Name"
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase)) 
                {
                    walks =
                        walks
                        // .Where(x => x.Name.Contains(filterQuery));
                        // Không phân biệt hoa / thường "Keyword"
                        .Where(walk => 
                            EF.Functions.ILike(walk.Name, $"%{filterQuery}%")
                        );
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                // Order by Name
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(walk => walk.Name) : walks.OrderByDescending(walk => walk.Name);
                }

                // Order by Length
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(walk => walk.LengthInKm) : walks.OrderByDescending(walk => walk.LengthInKm);
                }

            }

            // Pagination
            // skipResults: Số phần tử bỏ qua, vd: pageNumber = 3, pageSize = 10
            // skipResults = (3 - 1) * 10 = 20 => Bỏ qua 20 phần tử đầu tiên
            var skipResults = (pageNumber - 1) * pageSize;

            // Bỏ qua skipResults phần tử đầu tiên, lấy pageSize phần tử tiếp theo
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
            // return await _dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetById(Guid id)
        {
            return await 
                _dbContext
                .Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Walk?> Update(Walk walk, Guid id)
        {
            var walkFound = await 
                _dbContext
                .Walks
                .Include(w => w.Region)
                .Include(w => w.Difficulty)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (walkFound == null)
            {
                return null;
            }

            walkFound.Name = walk.Name;
            walkFound.Description = walk.Description;
            walkFound.LengthInKm = walk.LengthInKm;
            walkFound.WalkImageUrl = walk.WalkImageUrl;
            walkFound.DifficultyId = walk.DifficultyId;
            walkFound.RegionId = walk.RegionId;

            await _dbContext.SaveChangesAsync();
            return walkFound;
        }
    }
}

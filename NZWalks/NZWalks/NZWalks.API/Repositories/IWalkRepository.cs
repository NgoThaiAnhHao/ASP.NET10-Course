using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAll(
            string? filterOn = null, 
            string? filterQuery = null, 
            string? sortBy = null, 
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 100
        );

        Task<Walk?> GetById(Guid id);

        Task<Walk> Create(Walk walk);

        Task<Walk?> Update(Walk walk, Guid id);

        Task<Walk?> Delete(Guid id);
    }
}

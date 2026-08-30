using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext: DbContext
    {
        public NZWalksDbContext(DbContextOptions dbContextOptions) :base(dbContextOptions)
        {
               
        }

        // Cho phép NZWalksDbContext quản lý và truy vấn các Difficulty trong database.
        public DbSet<Difficulty> Difficulty { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }
    }
}

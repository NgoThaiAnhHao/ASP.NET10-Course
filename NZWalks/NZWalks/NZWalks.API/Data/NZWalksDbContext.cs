using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        // Cho phép NZWalksDbContext quản lý và truy vấn các Difficulty trong database.
        public DbSet<Difficulty> Difficulty { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("37142dad-1f06-478b-88bb-2d2a23bd7d21"),
                    Name = "Easy"
                },

                new Difficulty()
                {
                    Id = Guid.Parse("416fb7e7-74ce-42cd-8187-bfe4b72cc266"),
                    Name = "Medium"
                },

                new Difficulty()
                {
                    Id = Guid.Parse("a10f2874-f754-48fc-8782-fc1b4d88a85e"),
                    Name = "Hard"
                },
            };

            // Seed data for Regions
            var regions = new List<Region>()
            {
                new Region
                {
                    Id = Guid.Parse("bda60134-8884-4c35-a919-9bce0470deb9"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://tse2.mm.bing.net/th/id/OIP.PAlYd-juBzm50ed8uPN6JAHaE4?r=0&rs=1&pid=ImgDetMain&o=7&rm=3"
                },

                new Region
                {
                    Id = Guid.Parse("5e7fbf0a-caec-4854-a5ce-e0cb5f99cbca"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },

                new Region
                {
                    Id = Guid.Parse("552126bf-e417-4a49-90f3-798ddaa3d411"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://tse3.mm.bing.net/th/id/OIP.3lxQX-ol-rT4Sy0cJCl2MAHaEK?r=0&rs=1&pid=ImgDetMain&o=7&rm=3"
                },

                new Region
                {
                    Id = Guid.Parse("6e0e67e0-eddc-499f-8c74-e6bc4fde87c5"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = null
                },

                new Region
                {
                    Id = Guid.Parse("b1e47b7a-b9b7-4269-a5f3-3dae2dc68f5c"),
                    Name = "SounthLand",
                    Code = "STL",
                    RegionImageUrl = null
                }
            };

            // Seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed regions to the database
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}

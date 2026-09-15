using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NZWalksDbContext _nZWalksDbContext;

        public LocalImageRepository(
            IWebHostEnvironment webHostEnvironment, 
            IHttpContextAccessor httpContextAccessor,
            NZWalksDbContext nZWalksDbContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._httpContextAccessor = httpContextAccessor;
            this._nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(
                _webHostEnvironment.ContentRootPath,
                "images",
                $"{image.FileName}{image.FileExtension}"
            );

            // Upload image to local path
            // using declaration, dùng để tự động giải phóng tài nguyên khi không còn sử dụng stream
            using var stream = new FileStream(
                localFilePath,
                FileMode.Create
            );

            await image.File.CopyToAsync(stream);

            // https://localhost:portnumber/images/image.jpg
            var urlFilePath =
                $"{_httpContextAccessor.HttpContext.Request.Scheme}" +
                $"://{_httpContextAccessor.HttpContext.Request.Host}" +
                $"{_httpContextAccessor.HttpContext.Request.PathBase}" +
                $"/images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            // Add image to the Images table
            await _nZWalksDbContext.Images.AddAsync(image);
            await _nZWalksDbContext.SaveChangesAsync();

            return image;
        }
    }
}

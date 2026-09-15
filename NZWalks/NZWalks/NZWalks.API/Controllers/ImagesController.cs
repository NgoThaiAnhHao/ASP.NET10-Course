using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Image;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this._imageRepository = imageRepository;
        }

        // POST: /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequest imageUploadRequest)
        {
            ValidateFileUpload(imageUploadRequest);

            if (ModelState.IsValid)
            {
                // Convert dto to domain model
                var image = new Image
                {
                    File = imageUploadRequest.File,
                    FileName = imageUploadRequest.FileName,
                    FileDescription = imageUploadRequest.FileDescription,
                    FileExtension = Path.GetExtension(imageUploadRequest.File.FileName),
                    FileSizeInBytes = imageUploadRequest.File.Length
                };

                // Save to db
                await _imageRepository.Upload(image);

                return Ok(image );
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequest imageUploadRequest) {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            // Check extension
            if (!allowedExtensions.Contains(
                    Path.GetExtension(imageUploadRequest.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extensions.");
            }

            // Check size > 10mb
            if (imageUploadRequest.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
            }


        }
    }
}

using Microsoft.AspNetCore.Http;

namespace MyBlog.Persistance.Repositories.ImageRepository
{
    public class UploadImageRequest
    {
        public int PostId { get; set; }
        public IFormFile ImageFile { get; set; } = null!;
    }
}

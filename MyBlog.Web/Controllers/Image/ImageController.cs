using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.ImageRepository;
using MyBlog.Web.ViewModels.Image;

namespace MyBlog.Web.Controllers.Image
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageManager imageManager;

        public ImageController(IImageManager imageManager)
        {
            this.imageManager = imageManager;
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(string username, string filename, IFormFile blob)
        {
            var request = new UploadAvatarRequest
            {
                Username = username,
                ImageName = filename,
                ImageFile = blob
            };

            var result = await imageManager.UploadUserAvatarAsync(request);
            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.ImageRepository;
using MyBlog.Web.ViewModels.Image;
using SixLabors.ImageSharp.Formats;
using System.Reflection.Metadata;

namespace MyBlog.Web.Controllers.Image
{
    [ApiController]
    [Produces("application/json")]
    public class ImageController : ControllerBase
    {
        private readonly IImageManager imageManager;

        public ImageController(IImageManager imageManager)
        {
            this.imageManager = imageManager;
        }

        [Route("api/avatars/{username}")]
        [HttpGet]
        public async Task<string> GetAvatar(string username)
        {
            var base64String = await imageManager.GetAvatarAsync(username);

            return base64String;
        }

        [Route("api/avatars/upload")]
        [Route("api/avatars/upload/{username}")]
        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile blob, string? username)
        {
            if(username == null)
            {
                username = User.Identity!.Name;
            }

            if (User.Identity.Name != username)
            {
                return Forbid();
            }

            var request = new UploadAvatarRequest
            {
                Username = username!,
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

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MyBlog.Persistance.Repositories.ImageRepository;
//using MyBlog.Web.ViewModels.Image;

//namespace MyBlog.Web.Controllers.Image
//{
//    public class ImageController : Controller
//    {
//        private readonly IImageManager imageManager;

//        public ImageController(IImageManager imageManager)
//        {
//            this.imageManager = imageManager;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }
//        public IActionResult LoadImageTemplate()
//        {
//            return View();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpGet]
//        public IActionResult UploadAvatar(string username)
//        {
//            var avatarViewModel = new AvatarViewModel {
//                Username = username
//            };
//            return PartialView("_UploadUserAvatar", avatarViewModel);
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> UploadAvatar(string username, string filename, IFormFile blob)
//        {
            
//            var request = new UploadAvatarRequest
//            {
//                Username = username,
//                ImageName = filename,
//                ImageFile = blob
//            };

//            var result = await imageManager.UploadUserAvatarAsync(request);
//            if (result)
//            {
//                //return RedirectToAction("Index", new { id = id });
//                return NoContent();
//            }
//            else
//            {
//                return NoContent();
//            }
//        }
//    }
//}

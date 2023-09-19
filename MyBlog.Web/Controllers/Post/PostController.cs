using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyBlog.Persistance.Repositories.ImageRepository;
using MyBlog.Persistance.Repositories.PostRepository;
using MyBlog.Web.ViewModels.Accounts;
using MyBlog.Web.ViewModels.Image;
using MyBlog.Web.ViewModels.Post;

namespace MyBlog.Web.Controllers.Post
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly IPostManager postManager;
        private readonly IImageManager imageManager;

        public PostController(IPostManager postManager,
            IImageManager imageManager)
        {
            this.postManager = postManager;
            this.imageManager = imageManager;
        }


        [HttpGet]
        public async Task<ActionResult<PostViewModel>> Index(int id)
        {
            var response = await postManager.GetByIdAsync(id);

            var post = new PostViewModel
            {
                Id = id,
                Title = response.Title,
                Text = response.Text,
                PublishDate = response.PublishDate,
                AuthorsName = response.AuthorsName,
                ImagesNames = response.ImagesNames
            };

            post.SavesCount = await postManager.SavesCountAsync(id);

            //if (User.Identity == null || !User.Identity.IsAuthenticated)
            //{
            //    return PartialView("_IndexPost", post);
            //}

            //post.IsSaved = await postManager.IsSavedAsync(id, User.Identity.Name!);

            return Ok(post);
        }


        [HttpGet]
        public async Task<ActionResult<PostsPageViewModel>> AllPosts(int page = 1, string search = "")
        {
            var response = string.IsNullOrEmpty(search)
                ? await postManager.GetAllAsync(page)
                : await postManager.GetAllBySearchAsync(page, search!);

            if (response.Posts == null || response.Posts.Count() == 0)
            {
                return NoContent();
            }

            var postVm = new PostsPageViewModel
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Posts = new List<PostViewModel>()
            };

            foreach (var p in response.Posts)
            {
                postVm.Posts.Add(new PostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    PublishDate = p.PublishDate,
                    AuthorsName = p.AuthorsName,
                    ImagesNames = p.ImagesNames
                });
            }

            postVm.Posts = postVm.Posts.OrderByDescending(x => x.PublishDate).ToList();

            return Ok(postVm);
        }

        [HttpGet]
        public async Task<ActionResult<PostViewModel>> PostCard(int id)
        {
            var response = await postManager.GetByIdAsync(id);

            var post = new PostViewModel
            {
                Id = id,
                Title = response.Title,
                Text = response.Text,
                PublishDate = response.PublishDate,
                AuthorsName = response.AuthorsName,
                ImagesNames = response.ImagesNames
            };

            return Ok(post);
        }

        [HttpGet]
        public async Task<ActionResult<PostsPageViewModel>> UserPosts(string username, int page = 1)
        {
            var response = await postManager.GetAllByAuthorAsync(username, page);

            //ViewBag.IsSavedPosts = 0;

            if (response.Posts == null || response.Posts.Count() == 0)
            {
                return NoContent();
            }

            var postVm = new PostsPageViewModel
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Posts = new List<PostViewModel>()
            };

            foreach (var p in response.Posts)
            {
                postVm.Posts.Add(new PostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    PublishDate = p.PublishDate,
                    AuthorsName = p.AuthorsName,
                    ImagesNames = p.ImagesNames
                });
            }

            postVm.Posts = postVm.Posts.OrderByDescending(x => x.PublishDate).ToList();

            return Ok(postVm);
        }

        [HttpGet]
        public async Task<ActionResult<PostsPageViewModel>> UserSavedPosts(string username, int page = 1)
        {
            var response = await postManager.GetAllSavedAsync(username, page);

            //ViewBag.IsSavedPosts = 1;
            if (response.Posts == null || response.Posts.Count() == 0)
            {
                return NoContent();
            }

            var postVm = new PostsPageViewModel
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Posts = new List<PostViewModel>()
            };

            foreach (var p in response.Posts)
            {
                postVm.Posts.Add(new PostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    PublishDate = p.PublishDate,
                    AuthorsName = p.AuthorsName,
                    ImagesNames = p.ImagesNames
                });
            }

            postVm.Posts = postVm.Posts.OrderByDescending(x => x.PublishDate).ToList();

            return Ok(postVm);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        public async Task<IActionResult> SavePost(int id)
        {
            var result = await postManager.AddToSavedAsync(id, User.Identity!.Name!);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFormSavePost(int id)
        {
            var result = await postManager.DeleteFromSavedAsync(id, User.Identity!.Name!);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            if (User.Identity == null)
            {
                return Unauthorized();
            }

            postViewModel.AuthorsName = User.Identity.Name!;

            ModelState.Remove("AuthorsName");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var request = new CreatePostRequest
            {
                Title = postViewModel.Title,
                Text = postViewModel.Text,
                AuthorsName = postViewModel.AuthorsName
            };

            var result = Task.Run(async () => await postManager.CreateAsync(request)).Result;


            if (postViewModel.Images != null)
            {
                var imageList = new List<UploadImageRequest>();
                foreach (var image in postViewModel.Images)
                {
                    var imageRequest = new UploadImageRequest
                    {
                        ImageFile = image,
                        PostId = result
                    };

                    imageList.Add(imageRequest);
                }
                var loadingImages = await imageManager.UploadPostImagesAsync(imageList);
            }

            if (result > 0)
                return Ok();
            else
                return BadRequest();
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet]
        public async Task<ActionResult<ImagesNamesViewModel>> GetPostImagesNames(int postid)
        {
            var imagesNames = await imageManager.GetImagesNamesByPostIdAsync(postid);

            var imagesNamesViewModel = new ImagesNamesViewModel
            {
                PostId = postid,
                ImagesNames = imagesNames
            };

            return Ok(imagesNamesViewModel);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpDelete]
        public async Task<IActionResult> DeletePostImage(string imagename)
        {
            var result = await imageManager.DeletePostImageAsync(imagename);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        public async Task<IActionResult> UploadPostImage(int postid, IFormFile image)
        {

            var imageRequest = new UploadImageRequest
            {
                ImageFile = image,
                PostId = postid
            };

            var result = await imageManager.UploadPostImageAsync(imageRequest);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpGet]
        public async Task<ActionResult<EditPostViewModel>> Update(int id)
        {
            var response = await postManager.GetByIdAsync(id);

            var post = new EditPostViewModel
            {
                Id = id,
                Title = response.Title,
                Text = response.Text,
                PublishDate = response.PublishDate,
                AuthorsName = response.AuthorsName,
                ImagesNames = response.ImagesNames
            };

            return Ok(post);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPut]
        public async Task<IActionResult> Update(EditPostViewModel postViewModel)
        {
            if (User.Identity == null)
            {
                return Unauthorized(postViewModel);
            }
            postViewModel.AuthorsName = User.Identity.Name!;

            ModelState.Remove("AuthorsName");
            if (!ModelState.IsValid)
            {
                return BadRequest(postViewModel);
            }

            var request = new UpdatePostRequest
            {
                Id = postViewModel.Id,
                Title = postViewModel.Title,
                Text = postViewModel.Text,
            };

            var result = await postManager.UpdateAsync(request);
            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await postManager.GetByIdAsync(id);

            if (response.ImagesNames != null)
            {
                var deletingImages = Task.Run(async () => await imageManager.DeletePostImagesAsync(id)).Result;
            }

            var result = await postManager.DeleteAsync(id);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

    }
}

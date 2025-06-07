using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.ImageRepository;
using MyBlog.Persistance.Repositories.PostRepository;
using MyBlog.Web.Dto.Post;

namespace MyBlog.Web.Controllers.Post
{
    [ApiController]
    [Produces("application/json")]
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

        [Route("api/post/{id}")]
        [HttpGet]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            var response = await postManager.GetByIdAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            var post = new PostDto
            {
                Id = id,
                Title = response.Title,
                Text = response.Text,
                PublishDate = response.PublishDate,
                AuthorsName = response.AuthorsName,
                SumIds = response.SumIds
            };

            post.Images64s = await imageManager.GetImages64ByPostIdAsync(id);

            post.SavesCount = await postManager.SavesCountAsync(id);

            return Ok(post);
        }


        [Route("api/all-posts")]
        [Route("api/all-posts/{page?}")]
        [Route("api/all-posts/{search?}/{page?}")]
        [HttpGet]
        public async Task<ActionResult<PostsPageDto>> AllPosts(int page = 1, string search = "")
        {
            var response = string.IsNullOrEmpty(search)
                ? await postManager.GetAllAsync(page)
                : await postManager.GetAllBySearchAsync(page, search!);

            if (response.Posts == null || response.Posts.Count() == 0)
            {
                return NoContent();
            }

            var postsPageVm = new PostsPageDto
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Posts = new List<PostDto>()
            };

            foreach (var p in response.Posts)
            {
                var postVm = new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    PublishDate = p.PublishDate,
                    AuthorsName = p.AuthorsName,
                };

                postVm.Images64s = await imageManager.GetImages64ByPostIdAsync(p.Id, true);

                postsPageVm.Posts.Add(postVm);
            }

            postsPageVm.Posts = postsPageVm.Posts.OrderByDescending(x => x.PublishDate).ToList();

            return Ok(postsPageVm);
        }

        [Route("api/posts/{username}")]
        [Route("api/posts/{username}/{page?}")]
        [HttpGet]
        public async Task<ActionResult<PostsPageDto>> UserPosts(string username, int page = 1)
        {
            var response = await postManager.GetAllByAuthorAsync(username, page);

            if (response.Posts == null || response.Posts.Count() == 0)
            {
                return NoContent();
            }

            var postsPageVm = new PostsPageDto
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Posts = new List<PostDto>()
            };

            foreach (var p in response.Posts)
            {
                var postVm = new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    PublishDate = p.PublishDate,
                    AuthorsName = p.AuthorsName,
                };

                postVm.Images64s = await imageManager.GetImages64ByPostIdAsync(p.Id, true);

                postsPageVm.Posts.Add(postVm);
            }

            postsPageVm.Posts = postsPageVm.Posts.OrderByDescending(x => x.PublishDate).ToList();

            return Ok(postsPageVm);
        }

        [Route("api/saved-posts/{username}")]
        [Route("api/saved-posts/{username}/{page?}")]
        [HttpGet]
        public async Task<ActionResult<PostsPageDto>> UserSavedPosts(string username, int page = 1)
        {
            var response = await postManager.GetAllSavedAsync(username, page);

            if (response.Posts == null || response.Posts.Count() == 0)
            {
                return NoContent();
            }

            var postsPageVm = new PostsPageDto
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Posts = new List<PostDto>()
            };

            foreach (var p in response.Posts)
            {
                var postVm = new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Text = p.Text,
                    PublishDate = p.PublishDate,
                    AuthorsName = p.AuthorsName,
                };

                postVm.Images64s = await imageManager.GetImages64ByPostIdAsync(p.Id, true);

                postsPageVm.Posts.Add(postVm);
            }

            postsPageVm.Posts = postsPageVm.Posts.OrderByDescending(x => x.PublishDate).ToList();

            return Ok(postsPageVm);
        }

        [Route("api/post/is-saved/{id}")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<bool>> IsSaved(int id)
        {
            var isSaved = await postManager.IsSavedAsync(id, User.Identity!.Name!);

            return Ok(isSaved);
        }

        [Route("api/save-post/{id}")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SavePost(int id)
        {
            var result = await postManager.AddToSavedAsync(id, User.Identity!.Name!);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Route("api/delete-saved/{id}")]
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteFormSavePost(int id)
        {
            var result = await postManager.DeleteFromSavedAsync(id, User.Identity!.Name!);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Route("api/create-post")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromForm] CreatePostRequest request, IEnumerable<IFormFile>? images)
        {
            if (User.Identity == null)
            {
                return Unauthorized();
            }

            ModelState.Remove("AuthorsName");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            request.AuthorsName = User.Identity.Name!;

            var result = await postManager.CreateAsync(request);


            if (images != null && images.Any() && result > 0)
            {
                var imageList = new List<UploadImageRequest>();
                foreach (var image in images)
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
                return Ok(result);
            else
                return BadRequest();
        }

        [Route("api/edit-post/")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePostRequest request, [FromForm] string username,
            IEnumerable<IFormFile>? images)
        {
            if (User.Identity == null)
            {
                return Unauthorized(request);
            }

            if (User.Identity.Name != username)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            var result = await postManager.UpdateAsync(request);

            await imageManager.DeletePostImagesAsync(request.Id);

            if (images != null && images.Any() && result)
            {
                var imageList = new List<UploadImageRequest>();
                foreach (var image in images)
                {
                    var imageRequest = new UploadImageRequest
                    {
                        ImageFile = image,
                        PostId = request.Id
                    };

                    imageList.Add(imageRequest);
                }
                var loadingImages = await imageManager.UploadPostImagesAsync(imageList);
            }

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Route("api/delete-post/{id}")]
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await imageManager.DeletePostImagesAsync(id);

            var result = await postManager.DeleteAsync(id);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

    }
}

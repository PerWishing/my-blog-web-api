//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using MyBlog.Persistance.Repositories.ImageRepository;
//using MyBlog.Persistance.Repositories.PostRepository;
//using MyBlog.Web.ViewModels.Accounts;
//using MyBlog.Web.ViewModels.Image;
//using MyBlog.Web.ViewModels.Post;

//namespace MyBlog.Web.Controllers.Post
//{
//    public class PostController : Controller
//    {
//        private readonly IPostManager postManager;
//        private readonly IImageManager imageManager;

//        public PostController(IPostManager postManager,
//            IImageManager imageManager)
//        {
//            this.postManager = postManager;
//            this.imageManager = imageManager;
//        }

//        [HttpGet]
//        public IActionResult PostTemplate()
//        {
//            return View();
//        }

//        [HttpGet]
//        public IActionResult UserPostsFeedTemplate()
//        {
//            return View();
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index(int id)
//        {
//            var response = await postManager.GetByIdAsync(id);

//            var post = new PostViewModel
//            {
//                Id = id,
//                Title = response.Title,
//                Text = response.Text,
//                PublishDate = response.PublishDate,
//                AuthorsName = response.AuthorsName,
//                ImagesNames = response.ImagesNames
//            };

//            post.SavesCount = await postManager.SavesCountAsync(id);

//            if (User.Identity == null || !User.Identity.IsAuthenticated)
//            {
//                return PartialView("_IndexPost", post);
//            }

//            post.IsSaved = await postManager.IsSavedAsync(id, User.Identity.Name!);

//            return PartialView("_IndexPost", post);
//        }

//        [HttpGet]
//        public async Task<IActionResult> SavePartial(int id, string postauthor)
//        {
//            var post = new PostViewModel 
//            { 
//                Id = id,
//                AuthorsName = postauthor
//            };

//            post.SavesCount = await postManager.SavesCountAsync(id);

//            if (User.Identity == null || !User.Identity.IsAuthenticated)
//            {
//                return PartialView("_SavePartial", post);
//            }

//            post.IsSaved = await postManager.IsSavedAsync(id, User.Identity.Name!);

//            return PartialView("_SavePartial", post);
//        }

//        [HttpGet]
//        public async Task<IActionResult> AllPosts(int page = 1, string search = "")
//        {
//            var response = string.IsNullOrEmpty(search) 
//                ? await postManager.GetAllAsync(page) 
//                : await postManager.GetAllBySearchAsync(page, search!);

//            var posts = new List<PostViewModel>();

//            if (response.Posts != null && response.Posts.Count() > 0)
//            {
//                ViewBag.CurrentPage = page;
//                ViewBag.PageCount = response.PageCount;
//                foreach (var p in response.Posts)
//                {
//                    posts.Add(new PostViewModel
//                    {
//                        Id = p.Id,
//                        Title = p.Title,
//                        Text = p.Text,
//                        PublishDate = p.PublishDate,
//                        AuthorsName = p.AuthorsName,
//                        ImagesNames = p.ImagesNames
//                    });
//                }

//                posts = posts.OrderByDescending(x => x.PublishDate).ToList();
//            }

//            return PartialView("_AllPosts", posts);
//        }

//        [HttpGet]
//        public async Task<IActionResult> PostCard(int id)
//        {
//            var response = await postManager.GetByIdAsync(id);

//            var post = new PostViewModel
//            {
//                Id = id,
//                Title = response.Title,
//                Text = response.Text,
//                PublishDate = response.PublishDate,
//                AuthorsName = response.AuthorsName,
//                ImagesNames = response.ImagesNames
//            };

//            return PartialView("_Card", post);
//        }

//        [HttpGet]
//        public async Task<IActionResult> UserPosts(string username, int page = 1)
//        {
//            var response = await postManager.GetAllByAuthorAsync(username, page);

//            var posts = new List<PostViewModel>();

//            ViewBag.IsSavedPosts = 0;
//            if (response.Posts != null && response.Posts.Count() > 0)
//            {
//                ViewBag.CurrentPage = page;
//                ViewBag.PageCount = response.PageCount;
//                foreach (var p in response.Posts)
//                {
//                    posts.Add(new PostViewModel
//                    {
//                        Id = p.Id,
//                        Title = p.Title,
//                        Text = p.Text,
//                        PublishDate = p.PublishDate,
//                        AuthorsName = p.AuthorsName,
//                        ImagesNames = p.ImagesNames
//                    });
//                }

//                posts = posts.OrderByDescending(x => x.PublishDate).ToList();
//            }

//            return PartialView("_UserPosts", posts);
//        }

//        [HttpGet]
//        public async Task<IActionResult> UserSavedPosts(string username, int page = 1)
//        {
//            GetPostsPageResponse response = await postManager.GetAllSavedAsync(username, page);

//            IList<PostViewModel> posts = new List<PostViewModel>();

//            ViewBag.IsSavedPosts = 1;
//            if (response.Posts != null && response.Posts.Count() > 0)
//            {
//                ViewBag.CurrentPage = page;
//                ViewBag.PageCount = response.PageCount;
//                foreach (var p in response.Posts)
//                {
//                    posts.Add(new PostViewModel
//                    {
//                        Id = p.Id,
//                        Title = p.Title,
//                        Text = p.Text,
//                        PublishDate = p.PublishDate,
//                        AuthorsName = p.AuthorsName,
//                        ImagesNames = p.ImagesNames
//                    });
//                }

//                posts = posts.OrderByDescending(x => x.PublishDate).ToList();
//            }

//            return PartialView("_UserPosts", posts);
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> SavePost(int id)
//        {
//            await postManager.AddToSavedAsync(id, User.Identity!.Name!);

//            //return RedirectToAction("Index", new {id = id });
//            return NoContent();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> DeleteFormSavePost(int id)
//        {
//            await postManager.DeleteFromSavedAsync(id, User.Identity!.Name!);

//            //return RedirectToAction("Index", new { id = id });
//            return NoContent();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpGet]
//        public IActionResult Create()
//        {
//            return PartialView("_CreatePost");
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> Create(PostViewModel postViewModel)
//        {
//            if (User.Identity == null)
//            {
//                return View(postViewModel);
//            }

//            postViewModel.AuthorsName = User.Identity.Name!;

//            ModelState.Remove("AuthorsName");
//            if (!ModelState.IsValid)
//            {
//                return View(postViewModel);
//            }

//            var request = new CreatePostRequest
//            {
//                Title = postViewModel.Title,
//                Text = postViewModel.Text,
//                AuthorsName = postViewModel.AuthorsName
//            };

//            var result = Task.Run(async () => await postManager.CreateAsync(request)).Result;


//            if (postViewModel.Images != null)
//            {
//                var imageList = new List<UploadImageRequest>();
//                foreach (var image in postViewModel.Images)
//                {
//                    var imageRequest = new UploadImageRequest
//                    {
//                        ImageFile = image,
//                        PostId = result
//                    };

//                    imageList.Add(imageRequest);
//                }
//                var loadingImages = await imageManager.UploadPostImagesAsync(imageList);
//            }

//            if (result > 0)
//            {
//                //return RedirectToAction("Index", "User");
//                return NoContent();
//            }
//            else
//            {
//                return View(postViewModel);
//            }
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpGet]
//        public async Task<IActionResult> GetPostImagesNames(int postid)
//        {
//            var imagesNames = await imageManager.GetImagesNamesByPostIdAsync(postid);

//            var imagesNamesViewModel = new ImagesNamesViewModel
//            {
//                PostId = postid,
//                ImagesNames = imagesNames
//            };

//            return PartialView("_PostImagesEdit", imagesNamesViewModel);
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> DeletePostImage(string imagename)
//        {
//            var result = await imageManager.DeletePostImageAsync(imagename);

//            return NoContent();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> UploadPostImage(int postid, IFormFile image)
//        {

//            var imageRequest = new UploadImageRequest
//            {
//                ImageFile = image,
//                PostId = postid
//            };

//            await imageManager.UploadPostImageAsync(imageRequest);

//            return NoContent();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpGet]
//        public async Task<IActionResult> Update(int id)
//        {
//            if (User.Identity == null)
//            {
//                return View();
//            }

//            var response = await postManager.GetByIdAsync(id);

//            var post = new EditPostViewModel
//            {
//                Id = id,
//                Title = response.Title,
//                Text = response.Text,
//                PublishDate = response.PublishDate,
//                AuthorsName = response.AuthorsName,
//                ImagesNames = response.ImagesNames
//            };

//            return PartialView("_UpdatePost", post);
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> Update(int id, EditPostViewModel postViewModel)
//        {
//            if (User.Identity == null)
//            {
//                return View(postViewModel);
//            }
//            postViewModel.AuthorsName = User.Identity.Name!;

//            ModelState.Remove("AuthorsName");
//            if (!ModelState.IsValid)
//            {
//                return View(postViewModel);
//            }

//            var request = new UpdatePostRequest
//            {
//                Id = id,
//                Title = postViewModel.Title,
//                Text = postViewModel.Text,
//            };

//            var result = await postManager.UpdateAsync(request);
//            if (result)
//            {
//                //return RedirectToAction("Index", new { id = id });
//                return NoContent();
//            }
//            else
//            {
//                return View(postViewModel);
//            }
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var response = await postManager.GetByIdAsync(id);

//            if (response.ImagesNames != null)
//            {
//                var deletingImages = Task.Run(async () => await imageManager.DeletePostImagesAsync(id)).Result;
//            }

//            var result = await postManager.DeleteAsync(id);

//            return NoContent();
//        }

//    }
//}

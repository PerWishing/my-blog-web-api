//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MyBlog.Persistance.Repositories.CommentRepository;
//using MyBlog.Web.ViewModels.Comment;
//using MyBlog.Web.ViewModels.Post;

//namespace MyBlog.Web.Controllers.Comment
//{
//    public class CommentController : Controller
//    {
//        private readonly ICommentManager commentManager;

//        public CommentController(ICommentManager commentManager)
//        {
//            this.commentManager = commentManager;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpGet]
//        public IActionResult Create(int postId, string postauthor)
//        {
//            var commentViewModel = new CommentViewModel
//            {
//                PostId = postId,
//                PostAuthorsName = postauthor
//            };
//            return PartialView("_CreateComment", commentViewModel);
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> Create(CommentViewModel commentViewModel)
//        {
//            if (User.Identity == null)
//            {
//                return View(commentViewModel);
//            }

//            commentViewModel.AuthorsName = User.Identity.Name!;

//            ModelState.Remove("AuthorsName");
//            ModelState.Remove("PostAuthorsName");
//            if (!ModelState.IsValid)
//            {
//                return View(commentViewModel);
//            }

//            var request = new CreateCommentRequest
//            {
//                PostId = commentViewModel.PostId,
//                Text = commentViewModel.Text,
//                AuthorsName = commentViewModel.AuthorsName
//            };

//            var result = await commentManager.CreateAsync(request);

//            if (result)
//            {
//                return NoContent();
//            }
//            else
//            {
//                return View(commentViewModel);
//            }
//        }

//        [HttpGet]
//        public async Task<IActionResult> PostComments(int postId, string postauthor)
//        {
//            var response = await commentManager.GetAllByPostAsync(postId);

//            var comments = new List<CommentViewModel>();

//            foreach (var c in response)
//            {
//                var comment = new CommentViewModel
//                {
//                    Id = c.Id,
//                    Text = c.Text,
//                    Date = c.Date,
//                    AuthorsName = c.AuthorsName,
//                    PostId = c.PostId,
//                    PostAuthorsName = postauthor,
//                    LikesCount = c.LikesCount,
//                };

//                if (User.Identity != null && User.Identity.IsAuthenticated)
//                {
//                    comment.IsLiked = await commentManager.IsLikedAsync(c.Id, User.Identity.Name!);
//                }

//                comments.Add(comment);
//            }

//            comments = comments.OrderByDescending(x => x.Date).ToList();
//            comments = comments.OrderByDescending(x => x.LikesCount).ToList();

//            return PartialView("_PostComments", comments);
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await commentManager.DeleteAsync(id);

//            if (result)
//            {
//                return NoContent();
//            }
//            else
//            {
//                return NoContent();
//            }

//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> LikeComment(int id)
//        {
//            await commentManager.AddLikeAsync(id, User.Identity!.Name!);

//            //return RedirectToAction("Index", new {id = id });
//            return NoContent();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> DeleteLike(int id)
//        {
//            await commentManager.DeleteLikeAsync(id, User.Identity!.Name!);

//            //return RedirectToAction("Index", new { id = id });
//            return NoContent();
//        }
//    }
//}

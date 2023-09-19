using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.CommentRepository;
using MyBlog.Web.ViewModels.Comment;
using MyBlog.Web.ViewModels.Post;

namespace MyBlog.Web.Controllers.Comment
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentManager commentManager;

        public CommentController(ICommentManager commentManager)
        {
            this.commentManager = commentManager;
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        public async Task<IActionResult> Create(CommentViewModel commentViewModel)
        {
            if (User.Identity == null)
            {
                return Unauthorized();
            }

            commentViewModel.AuthorsName = User.Identity.Name!;

            ModelState.Remove("AuthorsName");
            ModelState.Remove("PostAuthorsName");
            if (!ModelState.IsValid)
            {
                return BadRequest(commentViewModel);
            }

            var request = new CreateCommentRequest
            {
                PostId = commentViewModel.PostId,
                Text = commentViewModel.Text,
                AuthorsName = commentViewModel.AuthorsName
            };

            var result = await commentManager.CreateAsync(request);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);

        }

        [HttpGet]
        public async Task<ActionResult<IList<CommentViewModel>>> PostComments(int postId, string postauthor)
        {
            var response = await commentManager.GetAllByPostAsync(postId);

            var comments = new List<CommentViewModel>();

            foreach (var c in response)
            {
                var comment = new CommentViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    Date = c.Date,
                    AuthorsName = c.AuthorsName,
                    PostId = c.PostId,
                    PostAuthorsName = postauthor,
                    LikesCount = c.LikesCount,
                };

                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    comment.IsLiked = await commentManager.IsLikedAsync(c.Id, User.Identity.Name!);
                }

                comments.Add(comment);
            }

            comments = comments.OrderByDescending(x => x.Date).ToList();
            comments = comments.OrderByDescending(x => x.LikesCount).ToList();

            return Ok(comments);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await commentManager.DeleteAsync(id);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        public async Task<IActionResult> LikeComment(int id)
        {
            var result = await commentManager.AddLikeAsync(id, User.Identity!.Name!);
            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var result = await commentManager.DeleteLikeAsync(id, User.Identity!.Name!);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }
    }
}

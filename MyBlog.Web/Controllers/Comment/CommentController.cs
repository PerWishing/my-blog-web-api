using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.CommentRepository;
using MyBlog.Web.Dto.Comment;

namespace MyBlog.Web.Controllers.Comment
{
    [ApiController]
    [Produces("application/json")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentManager commentManager;

        public CommentController(ICommentManager commentManager)
        {
            this.commentManager = commentManager;
        }

        [Route("api/comment/create")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentRequest request)
        {
            if (User.Identity == null)
            {
                return Unauthorized();
            }

            ModelState.Remove("AuthorsName");
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }

            request.AuthorsName = User.Identity.Name!; 

            var result = await commentManager.CreateAsync(request);

            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);

        }

        [Route("api/comments/{postId}")]
        [HttpGet]
        public async Task<ActionResult<IList<CommentDto>>> PostComments(int postId)
        {
            var response = await commentManager.GetAllByPostAsync(postId);

            var comments = new List<CommentDto>();

            foreach (var c in response)
            {
                var comment = new CommentDto
                {
                    Id = c.Id,
                    Text = c.Text,
                    Date = c.Date,
                    AuthorsName = c.AuthorsName,
                    PostId = c.PostId,
                    LikesCount = c.LikesCount,
                };

                comments.Add(comment);
            }

            comments = comments.OrderByDescending(x => x.Date).ToList();
            comments = comments.OrderByDescending(x => x.LikesCount).ToList();

            return Ok(comments);
        }

        [Route("api/comments/is-liked/{id}")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<bool>> IsLiked(int id)
        {
            var isLiked = await commentManager.IsLikedAsync(id, User.Identity!.Name!);

            return Ok(isLiked);
        }

        [Route("api/comments/delete/{id}")]
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await commentManager.DeleteAsync(id, User.Identity!.Name!);

            if (result)
                return Ok();
            else
                return BadRequest();
        }

        [Route("api/comments/like/{id}")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LikeComment(int id)
        {
            var result = await commentManager.AddLikeAsync(id, User.Identity!.Name!);
            if (result)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError/*, result.Error*/);
        }

        [Route("api/comments/unlike/{id}")]
        [Authorize]
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

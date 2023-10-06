using MyBlog.BlazorApp.Models.Comment;

namespace MyBlog.BlazorApp.Services.Comment
{
    public interface ICommentService
    {
        Task<IList<CommentVm>?> GetCommentsAsync(int postId);
        Task<string?> CreateCommentAsync(CreateCommentVm post);
        Task<string?> DeleteAsync(int id);
        Task<bool> IsLikedAsync(int id);
        Task<string?> LikeAsync(int id);
        Task<string?> UnlikeAsync(int id);
    }
}

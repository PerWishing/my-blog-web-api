using MyBlog.BlazorApp.Models.Comments;

namespace MyBlog.BlazorApp.Services.Comment
{
    public interface ICommentService
    {
        Task<IList<CommentDto>?> GetCommentsAsync(int postId);
        Task<string?> CreateCommentAsync(CreateCommentDto post);
        Task<string?> DeleteAsync(int id);
        Task<bool> IsLikedAsync(int id);
        Task<string?> LikeAsync(int id);
        Task<string?> UnlikeAsync(int id);
    }
}

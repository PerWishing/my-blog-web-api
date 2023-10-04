namespace MyBlog.Persistance.Repositories.CommentRepository
{
    public interface ICommentManager
    {
        Task<bool> CreateAsync(CreateCommentRequest request);
        Task<bool> DeleteAsync(int id, string username);
        Task<IList<GetCommentResponse>> GetAllByPostAsync(int postId);

        Task<bool> IsLikedAsync(int id, string username);
        Task<bool> AddLikeAsync(int id, string username);
        Task<bool> DeleteLikeAsync(int id, string username);
    }
}

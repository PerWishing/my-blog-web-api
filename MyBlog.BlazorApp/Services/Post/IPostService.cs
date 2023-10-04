using MyBlog.BlazorApp.Models.Post;

namespace MyBlog.BlazorApp.Services.Post
{
    public interface IPostService
    {
        Task<PostDto?> GetPostAsync(int id);
        Task<PostsPageDto?> GetAllPostsAsync(string search, int page = 1);
        Task<PostsPageDto?> GetUserPostsAsync(string username, int page = 1);
        Task<PostsPageDto?> GetUserSavedPostsAsync(string username, int page = 1);
        Task<bool> IsSavedPostAsync(int id);
        Task<string?> SavePostAsync(int id);
        Task<string?> DeleteSavedPostAsync(int id);
        Task<int?> CreatePostAsync(CreatePostDto post, IEnumerable<byte[]>? images);
        Task<string?> EditPostAsync(EditPostDto post, IEnumerable<byte[]>? images);
        Task<string?> DeletePostAsync(int id);
    }
}

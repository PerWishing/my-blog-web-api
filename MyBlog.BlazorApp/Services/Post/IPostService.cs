using MyBlog.BlazorApp.Models.Post;

namespace MyBlog.BlazorApp.Services.Post
{
    public interface IPostService
    {
        Task<PostVm?> GetPostAsync(int id);
        Task<PostsPageVm?> GetAllPostsAsync(string search, int page = 1);
        Task<PostsPageVm?> GetUserPostsAsync(string username, int page = 1);
        Task<PostsPageVm?> GetUserSavedPostsAsync(string username, int page = 1);
        Task<bool> IsSavedPostAsync(int id);
        Task<string?> SavePostAsync(int id);
        Task<string?> DeleteSavedPostAsync(int id);
        Task<int?> CreatePostAsync(CreatePostVm post, IEnumerable<byte[]>? images);
        Task<string?> EditPostAsync(EditPostVm post, IEnumerable<byte[]>? images);
        Task<string?> DeletePostAsync(int id);
        Task<int?> CreateSummarizationPostAsync(CreatePostVm post, IEnumerable<byte[]>? images);
        Task<Stream?> DownloadInputSummarizationAsync(int id);
        Task<Stream?> DownloadOutputSummarizationAsync(int id);
    }
}

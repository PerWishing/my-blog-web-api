namespace MyBlog.Persistance.Repositories.PostRepository
{
    public interface IPostManager
    {
        Task<int> SavesCountAsync(int id);
        Task<GetPostsPageResponse> GetAllSavedAsync(string username, int page);
        Task<bool> IsSavedAsync(int id, string username);
        Task<bool> AddToSavedAsync(int id, string username);
        Task<bool> DeleteFromSavedAsync(int id, string username);
        Task<GetPostsPageResponse> GetAllAsync(int page);
        Task<GetPostsPageResponse> GetAllByAuthorAsync(string author, int page);
        Task<GetPostsPageResponse> GetAllBySearchAsync(int page, string searchString);
        Task<GetPostResponse> GetByIdAsync(int id);
        Task<int> CreateAsync(CreatePostRequest request);
        Task<bool> UpdateAsync(UpdatePostRequest request);
        Task<bool> DeleteAsync(int id);
    }
}

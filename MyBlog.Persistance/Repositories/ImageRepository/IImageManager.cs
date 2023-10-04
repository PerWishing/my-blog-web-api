namespace MyBlog.Persistance.Repositories.ImageRepository
{
    public interface IImageManager
    {
        Task<IEnumerable<string>> GetImages64ByPostIdAsync(int id, bool firstOnly = false);
        Task<bool> UploadPostImagesAsync(IEnumerable<UploadImageRequest> requests);
        Task<bool> DeletePostImagesAsync(int id);
        Task<string> GetAvatarAsync(string username);
        Task<bool> UploadUserAvatarAsync(UploadAvatarRequest request);
        Task<bool> DeleteUserAvatarAsync(string username);
    }
}

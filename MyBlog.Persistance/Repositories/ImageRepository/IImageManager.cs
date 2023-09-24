namespace MyBlog.Persistance.Repositories.ImageRepository
{
    public interface IImageManager
    {
        Task<IEnumerable<string>> GetImagesNamesByPostIdAsync(int id);
        Task<bool> UploadPostImageAsync(UploadImageRequest request);
        Task<bool> DeletePostImageAsync(string imagename);
        Task<bool> UploadPostImagesAsync(IEnumerable<UploadImageRequest> requests);
        Task<bool> DeletePostImagesAsync(int id);
        Task<string> GetAvatarAsync(string username);
        Task<bool> UploadUserAvatarAsync(UploadAvatarRequest request);
        Task<bool> DeleteUserAvatarAsync(string username);
    }
}

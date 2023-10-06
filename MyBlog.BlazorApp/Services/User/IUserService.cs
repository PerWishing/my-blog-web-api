using MyBlog.BlazorApp.Models.User;

namespace MyBlog.BlazorApp.Services.User
{
    public interface IUserService
    {
        Task<string?> UploadAvatarAsync(byte[] blob);
        Task<UserVm?> GetUserByNameAsync(string username);
        Task<UsersPageVm?> GetUsersAsync(int page = 1, string? search = null);
        Task<UserVm?> GetUserAvatarAsync(string username);
        Task<string?> EditUserAsync(UserVm userVm);
        Task<string?> StartReadAsync(string username);
        Task<string?> StopReadAsync(string username);
        Task<IsReaderOrFollowedVm?> IsReaderOrFollowedAsync(string username);
        Task<UsersPageVm?> GetReadersAsync(string username, int page = 1, string? search = null);
        Task<UsersPageVm?> GetFollowedUsersAsync(string username, int page = 1, string? search = null);
        Task<JwtTokenVm?> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<string?> BlockUserAsync(string username);
        Task<string?> UnblockUserAsync(string username);
        Task<string?> GiveAdminAsync(string username);
        Task<string?> DeleteAdminAsync(string username);
        Task<string?> SignInAsync(LoginVm login);
        Task<bool> SignOutAsync(string username);
        Task<string?> RegisterAsync(RegisterVm register);
    }
}

using MyBlog.BlazorApp.Models;
using MyBlog.BlazorApp.Models.User;

namespace MyBlog.BlazorApp.Services.User.UserService
{
    public interface IUserService
    {
        Task<string?> UploadAvatarAsync(byte[] blob);
        Task<UserDto?> GetUserByNameAsync(string username);
        Task<UsersPageDto?> GetUsersAsync(int page = 1, string? search = null);
        Task<UserDto?> GetUserAvatarAsync(string username);
        Task<string?> EditUserAsync(UserDto userDto);
        Task<string?> StartReadAsync(string username);
        Task<string?> StopReadAsync(string username);
        Task<IsReaderOrFollowedDto?> IsReaderOrFollowedAsync(string username);
        Task<UsersPageDto?> GetReadersAsync(string username, int page = 1, string? search = null);
        Task<UsersPageDto?> GetFollowedUsersAsync(string username, int page = 1, string? search = null);
        Task<JwtTokenDto?> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<string?> SignInAsync(LoginDto login);
        Task<bool> SignOutAsync(string username);
        Task<string?> RegisterAsync(RegisterDto register);
    }
}

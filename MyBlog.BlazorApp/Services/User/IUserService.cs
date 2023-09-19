using MyBlog.BlazorApp.Models;

namespace MyBlog.BlazorApp.Services.User.UserService
{
    public interface IUserService
    {
        Task<UsersPageDto?> GetUsersAsync();
        Task<JwtTokenDto?> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<string?> SignInAsync(LoginDto login);
        Task<bool> SignOutAsync(string username);
        Task<bool> RegisterAsync(RegisterDto register);
    }
}

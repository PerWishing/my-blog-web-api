using MyBlog.Persistance.Identity;
using System.Security.Claims;

namespace MyBlog.Persistance.Repositories.UserRepository
{
    public interface IUserManager
    {
        Task<GetUsersPageResponse> GetAllUsersAsync(int page);
        Task<GetUsersPageResponse> GetUsersBySearchAsync(int page, string searchString);
        Task<GetUsersPageResponse> GetAllReadersAsync(string username, int page);
        Task<GetUsersPageResponse> GetReadersBySearchAsync(string username, int page, string searchString);
        Task<GetUsersPageResponse> GetAllFollowedAsync(string username, int page);
        Task<GetUsersPageResponse> GetFollowedBySearchAsync(string username, int page, string searchString);
        Task<bool> isFollowedByYouAsync(string username, string followedUserName);
        Task<bool> IsReaderAsync(string username, string readerName);
        Task<bool> BecomeReaderAsync(string readerName, string userOfBlogName);
        Task<bool> StopReadAsync(string username, string userOfBlogName);
        Task<GetUserResponse> GetByNameAsync(string username);
        Task<ApplicationUser> GetIdentityUserByNameAsync(string username);
        Task<bool> CreateAsync(string username, string password, string email);
        Task<bool> UpdateAsync(UpdateUserRequest request);
        Task<bool> BlockByNameAsync(string username);
        Task<bool> UnblockByNameAsync(string username);
        Task<bool> GiveAdminRoleAsync(string username);
        Task<bool> DeleteAdminRoleAsync(string username);
        Task<int> CheckPasswordSignInAsync(string username, string password);
        Task<bool> SignOutAsync();
        Task<IList<string>> GetRolesAsync(string username);
        Task<bool> SetUserRefreshTokenAsync(SetRefreshTokenRequest request);
        Task<bool> RevokeUserRefreshTokenAsync(string username);
    }
}

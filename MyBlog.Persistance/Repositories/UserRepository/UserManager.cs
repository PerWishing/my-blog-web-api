using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.Identity;
using MyBlog.Domain.Entities;
using System.Security.Claims;

namespace MyBlog.Persistance.Repositories.UserRepository
{
    public partial class UserManager : IUserManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly float pageResults = 4f;

        public UserManager(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public async Task<GetUsersPageResponse> GetAllUsersAsync(int page)
        {
            var queryCount = await context.Users
                .CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResults);

            var queryResult = await context.Users
                .Include(u => u.UserProfile)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var users = new List<GetUserResponse>();

            foreach (var item in queryResult)
            {
                var response = new GetUserResponse();
                                
                response.UserName = item.UserName!;
                response.AboutMyself = item.UserProfile.AboutMyself!;
                response.IsBlocked = item.UserProfile.IsBlocked;

                users.Add(response);
            }

            var pageResponse = new GetUsersPageResponse
            {
                PageCount = (int)pageCount,
                Users = users
            };
            return pageResponse;
        }

        public async Task<GetUsersPageResponse> GetUsersBySearchAsync(int page, string searchString)
        {
            searchString = searchString.ToUpper();

            var queryCount = await context.Users
                .Where(x => EF.Functions.Like(x.UserName!.ToUpper(), $"%{searchString}%"))
                .CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResults);

            var queryResult = await context.Users
                .Include(u => u.UserProfile)
                .Where(x => EF.Functions.Like(x.UserName!.ToUpper(), $"%{searchString}%"))
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var users = new List<GetUserResponse>();

            foreach (var item in queryResult)
            {
                var response = new GetUserResponse();
                                
                response.UserName = item.UserName!;
                response.AboutMyself = item.UserProfile.AboutMyself!;
                response.IsBlocked = item.UserProfile.IsBlocked;

                users.Add(response);
            }

            var pageResponse = new GetUsersPageResponse
            {
                PageCount = (int)pageCount,
                Users = users
            };
            return pageResponse;
        }

        public async Task<GetUsersPageResponse> GetAllReadersAsync(string username, int page)
        {
            var queryCount = await context.Readers
                .Include(x => x.Reader)
                .Where(x => x.User.UserName == username).CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResults);

            var queryResult = await context.Readers
                .Include(x => x.Reader)
                .Where(x => x.User.UserName == username)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var readers = new List<GetUserResponse>();

            foreach (var item in queryResult)
            {
                var response = new GetUserResponse();

                response.UserName = item.Reader.UserName!;
                response.IsBlocked = item.Reader.IsBlocked;

                readers.Add(response);
            }

            var pageResponse = new GetUsersPageResponse
            {
                PageCount = (int)pageCount,
                Users = readers
            };
            return pageResponse;
        }

        public async Task<GetUsersPageResponse> GetReadersBySearchAsync(string username, int page, string searchString)
        {
            searchString = searchString.ToUpper();

            var queryCount = await context.Readers
                .Include(x => x.Reader)
                .Where(x => x.User.UserName == username
                && EF.Functions.Like(x.Reader.UserName!.ToUpper(), $"%{searchString}%"))
                .CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResults);

            var queryResult = await context.Readers
                .Include(x => x.Reader)
                .Where(x => x.User.UserName == username
                && EF.Functions.Like(x.Reader.UserName!.ToUpper(), $"%{searchString}%"))
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var readers = new List<GetUserResponse>();

            foreach (var item in queryResult)
            {
                var response = new GetUserResponse();

                response.UserName = item.Reader.UserName!;
                response.IsBlocked = item.Reader.IsBlocked;

                readers.Add(response);
            }

            var pageResponse = new GetUsersPageResponse
            {
                PageCount = (int)pageCount,
                Users = readers
            };
            return pageResponse;
        }

        public async Task<GetUsersPageResponse> GetAllFollowedAsync(string username, int page)
        {
            var queryCount = await context.Readers
                .Include(x => x.User)
                .Where(x => x.Reader.UserName == username).CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResults);

            var queryResult = await context.Readers
                .Include(x => x.User)
                .Where(x => x.Reader.UserName == username)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var followedUsers = new List<GetUserResponse>();

            foreach (var item in queryResult)
            {
                var response = new GetUserResponse();

                response.UserName = item.User.UserName!;
                response.IsBlocked = item.User.IsBlocked;

                followedUsers.Add(response);
            }

            var pageResponse = new GetUsersPageResponse
            {
                PageCount = (int)pageCount,
                Users = followedUsers
            };
            return pageResponse;
        }

        public async Task<GetUsersPageResponse> GetFollowedBySearchAsync(string username, int page, string searchString)
        {
            searchString = searchString.ToUpper();

            var queryCount = await context.Readers
                .Include(x => x.User)
                .Where(x => x.Reader.UserName == username
                && EF.Functions.Like(x.User.UserName!.ToUpper(), $"%{searchString}%"))
                .CountAsync();

            var pageCount = Math.Ceiling(queryCount / pageResults);

            var queryResult = await context.Readers
                .Include(x => x.User)
                .Where(x => x.Reader.UserName == username
                && EF.Functions.Like(x.User.UserName!.ToUpper(), $"%{searchString}%"))
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var followedUsers = new List<GetUserResponse>();

            foreach (var item in queryResult)
            {
                var response = new GetUserResponse();

                response.UserName = item.User.UserName!;
                response.IsBlocked = item.User.IsBlocked;

                followedUsers.Add(response);
            }

            var pageResponse = new GetUsersPageResponse
            {
                PageCount = (int)pageCount,
                Users = followedUsers
            };
            return pageResponse;
        }

        public async Task<bool> isFollowedByYouAsync(string username, string followedUserName)
        {
            var isFollowedByYou = await context.Readers.Where(x =>
            x.Reader.UserName == username &&
            x.User.UserName == followedUserName).AnyAsync();

            if (isFollowedByYou)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> IsReaderAsync(string username, string readerName)
        {
            var isReader = await context.Readers.Where(x =>
            x.User.UserName == username &&
            x.Reader.UserName == readerName).AnyAsync();

            if (isReader)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> StopReadAsync(string username, string userOfBlogName)
        {
            var reader = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            var userOfBlog = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == userOfBlogName);

            var reader2user = new Readers();
            reader2user.ReaderId = reader!.Id;
            reader2user.Reader = reader!;
            reader2user.UserId = userOfBlog!.Id;
            reader2user.User = userOfBlog!;

            context.Readers.Remove(reader2user);

            var result = await context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> BecomeReaderAsync(string username, string userOfBlogName)
        {
            var reader = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            var userOfBlog = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == userOfBlogName);

            var reader2user = new Readers();
            reader2user.ReaderId = reader!.Id;
            reader2user.Reader = reader!;
            reader2user.UserId = userOfBlog!.Id;
            reader2user.User = userOfBlog!;

            context.Readers.Add(reader2user);

            var result = await context.SaveChangesAsync();

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<GetUserResponse> GetByNameAsync(string username)
        {
            var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            var response = new GetUserResponse
            {
                UserName = username,
                IsBlocked = user!.IsBlocked
            };
            if (user!.AboutMyself == null)
            {
                response.AboutMyself = "Sample text";
            }
            else
            {
                response.AboutMyself = user.AboutMyself;
            }

            return response;
        }

        public async Task<ApplicationUser> GetIdentityUserByNameAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            return user!;
        }

        public async Task<bool> UpdateAsync(UpdateUserRequest request)
        {
            var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == request.UserName);

            user!.AboutMyself = request.AboutMyself;

            var result = context.UserProfiles.Update(user);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateAsync(string username, string password, string email)
        {
            var checkIfExists = await userManager.FindByNameAsync(username);
            if (checkIfExists != null) { return false; }

            var user = new ApplicationUser
            {
                UserName = username,
                Email = email
            };

            user.UserProfile = new UserProfile
            {
                Id = user.Id,
                UserName = user.UserName
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));

                return true;
            }
            else { return false; }
        }

        public async Task<bool> BlockByNameAsync(string username)
        {
            var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            user!.IsBlocked = true;

            var result = context.UserProfiles.Update(user);

            return true;
        }

        public async Task<bool> UnblockByNameAsync(string username)
        {
            var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == username);

            user!.IsBlocked = false;

            var result = context.UserProfiles.Update(user);

            return true;
        }

        public async Task<int> CheckPasswordSignInAsync(string username, string password)
        {
            var user = await context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return 0;
            }

            if (user.UserProfile.IsBlocked)
            {
                return 3;
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public async Task<bool> SignOutAsync()
        {
            await signInManager.SignOutAsync();
            return true;
        }

        public async Task<IList<string>> GetRolesAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            var claims = await userManager.GetClaimsAsync(user!);

            IList<string> roles = new List<string>();

            foreach (var c in claims)
            {
                if (c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                {
                    roles.Add(c.Value);
                }
            }

            return roles;
        }

        public async Task<bool> SetUserRefreshTokenAsync(SetRefreshTokenRequest request)
        {
            var user = await userManager.FindByNameAsync(request.Username);

            user!.RefreshToken = request.RefreshToken;
            if (request.RefreshTokenExpiryTime != null)
            {
                user.RefreshTokenExpiryTime = (DateTime)request.RefreshTokenExpiryTime;
            }
            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> RevokeUserRefreshTokenAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
                return false;

            user.RefreshToken = null;
            
            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.UserRepository;
using MyBlog.Web.ViewModels.User;

namespace MyBlog.Web.Controllers.User
{
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [Route("api/user/reader-or-followed/{username}")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IsReaderOrFollowed(string username)
        {
            var isReader = await userManager.IsReaderAsync(User.Identity!.Name!, username);
            var isFollowed = await userManager.isFollowedByYouAsync(User.Identity!.Name!, username);

            var readerOrFollowedVm = new IsReaderOrFollowedVm
            {
                IsReader = isReader,
                IsFollowed = isFollowed,
            };

            return Ok(readerOrFollowedVm);
        }

        [Route("api/user/start-read/{username}")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BecomeReader(string username)
        {
            var result = await userManager.BecomeReaderAsync(User.Identity!.Name!, username);

            if (result)
                return Ok();
            else
                return BadRequest();
        }

        [Route("api/user/stop-read/{username}")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StopRead(string username)
        {
            var result = await userManager.StopReadAsync(User.Identity!.Name!, username);

            if (result)
                return Ok();
            else
                return BadRequest();
        }

        [Route("api/user/edit")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditUserProfile(UpdateUserRequest updateUserRequest)
        {
            if (updateUserRequest.UserName != User.Identity!.Name)
            {
                return Forbid();
            }

            var result = await userManager.UpdateAsync(updateUserRequest);

            if (result)
                return Ok();
            else
                return BadRequest(updateUserRequest);
        }

        [Route("api/user/{username}")]
        [HttpGet]
        public async Task<ActionResult<UserViewModel>> UserInfo(string username)
        {
            var user = await userManager.GetByNameAsync(username);

            var userViewModel = new UserViewModel
            {
                UserName = user.UserName,
                AboutMyself = user.AboutMyself,
                isBlocked = user.IsBlocked
            };

            var roles = await userManager.GetRolesAsync(username);
            userViewModel.isAdmin = roles.Where(r => r == "Admin").Any();

            return Ok(userViewModel);
        }

        [Route("api/users")]
        [Route("api/users/{page?}")]
        [Route("api/users/{search?}/{page?}")]
        [HttpGet]
        public async Task<ActionResult<UsersPageViewModel>> Users(int page = 1, string? search = null)
        {
            var response = string.IsNullOrEmpty(search)
                ? await userManager.GetAllUsersAsync(page)
                : await userManager.GetUsersBySearchAsync(page, search);

            if (response.Users == null || response.Users.Count() == 0)
            {
                return NoContent();
            }
            var usersPageVm = new UsersPageViewModel
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Users = new List<UserViewModel>()
            };
            foreach (var r in response.Users)
            {
                var userViewModel = new UserViewModel
                {
                    UserName = r.UserName,
                    isBlocked = r.IsBlocked
                };
                userViewModel.AboutMyself = r.AboutMyself ?? " ";

                usersPageVm.Users.Add(userViewModel);
            }

            return Ok(usersPageVm);
        }

        [Route("api/readers/{username}")]
        [Route("api/readers/{username}/{page?}")]
        [Route("api/readers/{username}/{search?}/{page?}")]
        [HttpGet]
        public async Task<ActionResult<UsersPageViewModel>> UserReaders(string username, int page = 1, string? search = null)
        {
            var response = string.IsNullOrEmpty(search)
                ? await userManager.GetAllReadersAsync(username, page)
                : await userManager.GetReadersBySearchAsync(username, page, search);

            if (response.Users == null || response.Users.Count() == 0)
            {
                return NoContent();
            }

            var readersPageVm = new UsersPageViewModel
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Users = new List<UserViewModel>()
            };

            foreach (var r in response.Users)
            {
                var userViewModel = new UserViewModel
                {
                    UserName = r.UserName,
                    isBlocked = r.IsBlocked
                };

                //if (r.IsBlocked || r.AvatarName == null)
                //{
                //    userViewModel.AvatarName = AvatarSample;
                //}
                //else
                //{
                //    userViewModel.AvatarName = r.AvatarName;
                //}

                readersPageVm.Users.Add(userViewModel);
            }

            return Ok(readersPageVm);
        }

        [Route("api/followed/{username}")]
        [Route("api/followed/{username}/{page?}")]
        [Route("api/followed/{username}/{search?}/{page?}")]
        [HttpGet]
        public async Task<ActionResult<UsersPageViewModel>> UserFollowedUsers(string username, int page = 1, string search = "")
        {

            var response = string.IsNullOrEmpty(search)
                ? await userManager.GetAllFollowedAsync(username, page)
                : await userManager.GetFollowedBySearchAsync(username, page, search);

            if (response.Users == null || response.Users.Count() == 0)
            {
                return NoContent();
            }

            var followedUsersPageVm = new UsersPageViewModel
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Users = new List<UserViewModel>()
            };

            foreach (var r in response.Users)
            {
                var userViewModel = new UserViewModel
                {
                    UserName = r.UserName,
                    isBlocked = r.IsBlocked
                };

                //if (r.IsBlocked || r.AvatarName == null)
                //{
                //    userViewModel.AvatarName = AvatarSample;
                //}
                //else
                //{
                //    userViewModel.AvatarName = r.AvatarName;
                //}

                followedUsersPageVm.Users.Add(userViewModel);
            }

            return Ok(followedUsersPageVm);
        }
    }
}

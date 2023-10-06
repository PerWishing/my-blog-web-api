using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.UserRepository;
using MyBlog.Web.Dto.User;

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

        [Route("api/user/{username}")]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUser(string username)
        {
            var user = await userManager.GetByNameAsync(username);

            var userDto = new UserDto
            {
                UserName = user.UserName,
                AboutMyself = user.AboutMyself,
                isBlocked = user.IsBlocked
            };

            var roles = await userManager.GetRolesAsync(username);
            userDto.isAdmin = roles.Where(r => r == "Admin").Any();

            return Ok(userDto);
        }

        [Route("api/users")]
        [Route("api/users/{page?}")]
        [Route("api/users/{search?}/{page?}")]
        [HttpGet]
        public async Task<ActionResult<UsersPageDto>> GetUsers(int page = 1, string? search = null)
        {
            var response = string.IsNullOrEmpty(search)
                ? await userManager.GetAllUsersAsync(page)
                : await userManager.GetUsersBySearchAsync(page, search);

            if (response.Users == null || response.Users.Count() == 0)
            {
                return NoContent();
            }
            var usersPageVm = new UsersPageDto
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Users = new List<UserDto>()
            };
            foreach (var r in response.Users)
            {
                var userDto = new UserDto
                {
                    UserName = r.UserName,
                    isBlocked = r.IsBlocked
                };
                userDto.AboutMyself = r.AboutMyself ?? " ";

                usersPageVm.Users.Add(userDto);
            }

            return Ok(usersPageVm);
        }

        [Route("api/readers/{username}")]
        [Route("api/readers/{username}/{page?}")]
        [Route("api/readers/{username}/{search?}/{page?}")]
        [HttpGet]
        public async Task<ActionResult<UsersPageDto>> GetReaders(string username, int page = 1, string? search = null)
        {
            var response = string.IsNullOrEmpty(search)
                ? await userManager.GetAllReadersAsync(username, page)
                : await userManager.GetReadersBySearchAsync(username, page, search);

            if (response.Users == null || response.Users.Count() == 0)
            {
                return NoContent();
            }

            var readersPageVm = new UsersPageDto
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Users = new List<UserDto>()
            };

            foreach (var r in response.Users)
            {
                var userDto = new UserDto
                {
                    UserName = r.UserName,
                    isBlocked = r.IsBlocked
                };

                readersPageVm.Users.Add(userDto);
            }

            return Ok(readersPageVm);
        }

        [Route("api/followed/{username}")]
        [Route("api/followed/{username}/{page?}")]
        [Route("api/followed/{username}/{search?}/{page?}")]
        [HttpGet]
        public async Task<ActionResult<UsersPageDto>> GetFollowedUsers(string username, int page = 1, string search = "")
        {

            var response = string.IsNullOrEmpty(search)
                ? await userManager.GetAllFollowedAsync(username, page)
                : await userManager.GetFollowedBySearchAsync(username, page, search);

            if (response.Users == null || response.Users.Count() == 0)
            {
                return NoContent();
            }

            var followedUsersPageVm = new UsersPageDto
            {
                CurrentPage = page,
                PageCount = response.PageCount,
                Users = new List<UserDto>()
            };

            foreach (var r in response.Users)
            {
                var userDto = new UserDto
                {
                    UserName = r.UserName,
                    isBlocked = r.IsBlocked
                };

                followedUsersPageVm.Users.Add(userDto);
            }

            return Ok(followedUsersPageVm);
        }

        [Route("api/user/edit")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> EditUser(UpdateUserRequest updateUserRequest)
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

        [Route("api/user/reader-or-followed/{username}")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IsReaderOrFollowed(string username)
        {
            var isReader = await userManager.IsReaderAsync(User.Identity!.Name!, username);
            var isFollowed = await userManager.isFollowedByYouAsync(User.Identity!.Name!, username);

            var readerOrFollowedDto = new IsReaderOrFollowedDto
            {
                IsReader = isReader,
                IsFollowed = isFollowed,
            };

            return Ok(readerOrFollowedDto);
        }

        [Route("api/user/start-read/{username}")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StartRead(string username)
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
    }
}

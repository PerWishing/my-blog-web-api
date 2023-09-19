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
        private string AvatarSample { get; }

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
            AvatarSample = "avatarSample.png";
        }

        //[HttpGet]
        //public async Task<IActionResult> UserPublicProfile(string username)
        //{
        //    var response = await userManager.GetByNameAsync(username);

        //    var userViewModel = new UserViewModel
        //    {
        //        UserName = response.UserName,
        //        isBlocked = response.IsBlocked
        //    };

        //    return Ok(userViewModel);
        //}

        //[Authorize(Policy = "UserPolicy")]
        //[HttpPost]
        //public async Task<IActionResult> BecomeReader(string readername)
        //{
        //    var result = await userManager.BecomeReaderAsync(User.Identity!.Name!, readername);

        //    if (result)
        //        return Ok();
        //    else
        //        return BadRequest();
        //}

        //[Authorize(Policy = "UserPolicy")]
        //[HttpPost]
        //public async Task<IActionResult> StopRead(string readername)
        //{
        //    var result = await userManager.StopReadAsync(User.Identity!.Name!, readername);

        //    if (result)
        //        return Ok();
        //    else
        //        return BadRequest();
        //}

        //[Authorize(Policy = "UserPolicy")]
        //[HttpGet]
        //public async Task<IActionResult> EditUserProfile(string username)
        //{
        //    var user = await userManager.GetByNameAsync(username);

        //    var userViewModel = new UserViewModel
        //    {
        //        UserName = user.UserName,
        //        AboutMyself = user.AboutMyself
        //    };

        //    return Ok(userViewModel);
        //}

        //[Authorize(Policy = "UserPolicy")]
        //[HttpPut]
        //public async Task<IActionResult> EditUserProfile(UserViewModel userViewModel)
        //{
        //    var request = new UpdateUserRequest
        //    {
        //        UserName = User.Identity!.Name!,
        //        AboutMyself = userViewModel.AboutMyself
        //    };

        //    var result = await userManager.UpdateAsync(request);

        //    if (result)
        //        return Ok();
        //    else
        //        return BadRequest(userViewModel);
        //}

        //[HttpGet]
        //public async Task<ActionResult<UserViewModel>> UserInfoPartial(string username)
        //{
        //    var user = await userManager.GetByNameAsync(username);

        //    var userViewModel = new UserViewModel
        //    {
        //        UserName = user.UserName,
        //        AboutMyself = user.AboutMyself,
        //        isBlocked = user.IsBlocked
        //    };

        //    if (user.IsBlocked || user.AvatarName == null)
        //    {
        //        userViewModel.AvatarName = AvatarSample;
        //    }
        //    else
        //    {
        //        userViewModel.AvatarName = user.AvatarName;
        //    }

        //    //userViewModel.isReader = await userManager.IsReaderAsync(User.Identity!.Name!, username);
        //    //userViewModel.isFollowedByYou = await userManager.isFollowedByYouAsync(User.Identity!.Name!, username);

        //    return Ok(userViewModel);
        //}

        [Route("api/users")]
        [Route("api/users/{page?}")]
        [Route("api/users/{page?}/{search?}")]
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
                if (r.IsBlocked || r.AvatarName == null)
                {
                    userViewModel.AvatarName = AvatarSample;
                }
                else
                {
                    userViewModel.AvatarName = r.AvatarName;
                }

                usersPageVm.Users.Add(userViewModel);
            }

            return Ok(usersPageVm);
        }

        //    [HttpGet]
        //    public async Task<ActionResult<UsersPageViewModel>> UserReaders(string username, int page = 1, string search = "")
        //    {
        //        var response = string.IsNullOrEmpty(search)
        //            ? await userManager.GetAllReadersAsync(username, page)
        //            : await userManager.GetReadersBySearchAsync(username, page, search);

        //        if (response.Users == null || response.Users.Count() == 0)
        //        {
        //            return NoContent();
        //        }

        //        var readersPageVm = new UsersPageViewModel
        //        {
        //            CurrentPage = page,
        //            PageCount = response.PageCount,
        //            Users = new List<UserViewModel>()
        //        };
        //        //ViewBag.ReaderOrFollowed = 0;

        //        foreach (var r in response.Users)
        //        {
        //            var userViewModel = new UserViewModel
        //            {
        //                UserName = r.UserName,
        //                isBlocked = r.IsBlocked
        //            };

        //            if (r.IsBlocked || r.AvatarName == null)
        //            {
        //                userViewModel.AvatarName = AvatarSample;
        //            }
        //            else
        //            {
        //                userViewModel.AvatarName = r.AvatarName;
        //            }

        //            readersPageVm.Users.Add(userViewModel);
        //        }


        //        return Ok(readersPageVm);
        //    }

        //    [HttpGet]
        //    public async Task<ActionResult<UsersPageViewModel>> UserFollowed(string username, int page = 1, string search = "")
        //    {

        //        var response = string.IsNullOrEmpty(search)
        //            ? await userManager.GetAllFollowedAsync(username, page)
        //            : await userManager.GetFollowedBySearchAsync(username, page, search);

        //        if (response.Users == null || response.Users.Count() == 0)
        //        {
        //            return NoContent();
        //        }

        //        var followedUsersPageVm = new UsersPageViewModel
        //        {
        //            CurrentPage = page,
        //            PageCount = response.PageCount,
        //            Users = new List<UserViewModel>()
        //        };

        //        //ViewBag.ReaderOrFollowed = 1;

        //        foreach (var r in response.Users)
        //        {
        //            var userViewModel = new UserViewModel
        //            {
        //                UserName = r.UserName,
        //                isBlocked = r.IsBlocked
        //            };

        //            if (r.IsBlocked || r.AvatarName == null)
        //            {
        //                userViewModel.AvatarName = AvatarSample;
        //            }
        //            else
        //            {
        //                userViewModel.AvatarName = r.AvatarName;
        //            }

        //            followedUsersPageVm.Users.Add(userViewModel);
        //        }


        //        return Ok(followedUsersPageVm);
        //    }
    }
}

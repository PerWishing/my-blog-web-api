//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MyBlog.Persistance.Repositories.UserRepository;
//using MyBlog.Web.ViewModels.User;

//namespace MyBlog.Web.Controllers.User
//{
//    public class UserController : Controller
//    {
//        private readonly IUserManager userManager;
//        private string AvatarSample { get; }

//        public UserController(IUserManager userManager)
//        {
//            this.userManager = userManager;
//            AvatarSample = "avatarSample.png";
//        }

//        [Authorize(Policy = "UserPolicy")]
//        public IActionResult Index()
//        {
//            return View();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpGet]
//        public IActionResult UserMain()
//        {
//            return PartialView("_UserMain");
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpGet]
//        public IActionResult MySavedPosts()
//        {
//            return PartialView("_MySavedPosts");
//        }

//        [HttpGet]
//        public async Task<IActionResult> UserPublicProfile(string username)
//        {
//            var response = await userManager.GetByNameAsync(username);

//            var userViewModel = new UserViewModel
//            {
//                UserName = response.UserName,
//                isBlocked = response.IsBlocked
//            };

//            return PartialView("_UserPublicProfile", userViewModel);
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> BecomeReader(string readername)
//        {
//            var result = await userManager.BecomeReaderAsync(User.Identity!.Name!, readername);

//            //return View("UserPublicProfile", readername);
//            return NoContent();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> StopRead(string readername)
//        {
//            var result = await userManager.StopReadAsync(User.Identity!.Name!, readername);

//            //return View("UserPublicProfile", readername);
//            return NoContent();
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpGet]
//        public async Task<IActionResult> EditUserProfile(string username)
//        {
//            var user = await userManager.GetByNameAsync(username);

//            var userViewModel = new UserViewModel
//            {
//                UserName = user.UserName,
//                AboutMyself = user.AboutMyself
//            };

//            return PartialView("_EditUserProfile", userViewModel);
//        }

//        [Authorize(Policy = "UserPolicy")]
//        [HttpPost]
//        public async Task<IActionResult> EditUserProfile(UserViewModel userViewModel)
//        {
//            var request = new UpdateUserRequest
//            {
//                UserName = User.Identity!.Name!,
//                AboutMyself = userViewModel.AboutMyself
//            };

//            var result = await userManager.UpdateAsync(request);


//            if (result)
//            {
//                //return RedirectToAction("Index");
//                return NoContent();
//            }
//            else
//            {
//                return View("EditUserProfile", userViewModel);
//            }
//        }

//        public IActionResult UserPostsMain()
//        {
//            return PartialView("_UserPostsMain");
//        }

//        public async Task<IActionResult> UserHead(string username)
//        {
//            var user = await userManager.GetByNameAsync(username);

//            var userViewModel = new UserViewModel
//            {
//                UserName = user.UserName,
//                isBlocked = user.IsBlocked
//            };

//            if (user.IsBlocked || user.AvatarName == null)
//            {
//                userViewModel.AvatarName = AvatarSample;
//            }
//            else
//            {
//                userViewModel.AvatarName = user.AvatarName;
//            }

//            return PartialView("_UserHeadPartial", userViewModel);
//        }

//        public async Task<IActionResult> UserInfoPartial(string username)
//        {
//            var user = await userManager.GetByNameAsync(username);

//            var userViewModel = new UserViewModel
//            {
//                UserName = user.UserName,
//                AboutMyself = user.AboutMyself,
//                isBlocked = user.IsBlocked
//            };

//            if (user.IsBlocked || user.AvatarName == null)
//            {
//                userViewModel.AvatarName = AvatarSample;
//            }
//            else
//            {
//                userViewModel.AvatarName = user.AvatarName;
//            }

//            if (User.Identity == null || !User.Identity.IsAuthenticated)
//            {
//                return PartialView("_UserInfo", userViewModel);
//            }

//            userViewModel.isReader = await userManager.IsReaderAsync(User.Identity!.Name!, username);
//            userViewModel.isFollowedByYou = await userManager.isFollowedByYouAsync(User.Identity!.Name!, username);


//            return PartialView("_UserInfo", userViewModel);
//        }

//        public async Task<IActionResult> Users(int page = 1, string search = "")
//        {
//            var response = string.IsNullOrEmpty(search)
//                ? await userManager.GetAllUsersAsync(page)
//                : await userManager.GetUsersBySearchAsync(page, search);

//            var userModels = new List<UserViewModel>();

//            if (response.Users != null && response.Users.Count() > 0)
//            {
//                ViewBag.CurrentPage = page;
//                ViewBag.PageCount = response.PageCount;
//                foreach (var r in response.Users)
//                {
//                    var userViewModel = new UserViewModel
//                    {
//                        UserName = r.UserName,
//                        isBlocked = r.IsBlocked
//                    };
//                    userViewModel.AboutMyself = r.AboutMyself ?? " ";
//                    if (r.IsBlocked || r.AvatarName == null)
//                    {
//                        userViewModel.AvatarName = AvatarSample;
//                    }
//                    else
//                    {
//                        userViewModel.AvatarName = r.AvatarName;
//                    }

//                    userModels.Add(userViewModel);
//                }
//            }

//            return PartialView("_Users", userModels);
//        }

//        public async Task<IActionResult> UserReaders(string username, int page = 1, string search = "")
//        {
//            var response = string.IsNullOrEmpty(search)
//                ? await userManager.GetAllReadersAsync(username, page)
//                : await userManager.GetReadersBySearchAsync(username, page, search);

//            var readerModels = new List<UserViewModel>();

//            ViewBag.ReaderOrFollowed = 0;
//            if (response.Users != null && response.Users.Count() > 0)
//            {
//                ViewBag.CurrentPage = page;
//                ViewBag.PageCount = response.PageCount;
//                foreach (var r in response.Users)
//                {
//                    var userViewModel = new UserViewModel
//                    {
//                        UserName = r.UserName,
//                        isBlocked = r.IsBlocked
//                    };

//                    if (r.IsBlocked || r.AvatarName == null)
//                    {
//                        userViewModel.AvatarName = AvatarSample;
//                    }
//                    else
//                    {
//                        userViewModel.AvatarName = r.AvatarName;
//                    }

//                    readerModels.Add(userViewModel);
//                }
//            }

//            return PartialView("_UserReadersOrFollowed", readerModels);
//        }

//        public async Task<IActionResult> UserFollowed(string username, int page = 1, string search = "")
//        {

//            var response = string.IsNullOrEmpty(search)
//                ? await userManager.GetAllFollowedAsync(username, page)
//                : await userManager.GetFollowedBySearchAsync(username, page, search);

//            var followedUserModels = new List<UserViewModel>();

//            ViewBag.ReaderOrFollowed = 1;
//            if (response.Users != null && response.Users.Count() > 0)
//            {
//                ViewBag.CurrentPage = page;
//                ViewBag.PageCount = response.PageCount;
//                foreach (var r in response.Users)
//                {
//                    var userViewModel = new UserViewModel
//                    {
//                        UserName = r.UserName,
//                        isBlocked = r.IsBlocked
//                    };

//                    if (r.IsBlocked || r.AvatarName == null)
//                    {
//                        userViewModel.AvatarName = AvatarSample;
//                    }
//                    else
//                    {
//                        userViewModel.AvatarName = r.AvatarName;
//                    }

//                    followedUserModels.Add(userViewModel);
//                }
//            }

//            return PartialView("_UserReadersOrFollowed", followedUserModels);
//        }
//    }
//}

//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using MyBlog.Web.ViewModels.Accounts;
//using System.Net;
//using System.Net.Mime;
//using MyBlog.Web.Models.Enums;
//using MyBlog.Persistance.Repositories.UserRepository;

//namespace MyBlog.Web.Controllers.Accounts
//{
//    public class AccountsController : Controller
//    {
//        private readonly IUserManager userManager;

//        public AccountsController(IUserManager userManager)
//        {
//            this.userManager = userManager;
//        }

//        [HttpGet]
//        public IActionResult Login()
//        {
//            return PartialView("_Login");
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return PartialView("_Login", loginViewModel);
//            }

//            if (loginViewModel.UserName == null || loginViewModel.Password == null)
//            {
//                return View();
//            }

//            var result = (LoginStatus)await userManager.SignInAsync(loginViewModel.UserName, loginViewModel.Password);

//            if (result == LoginStatus.Succeeded)
//            {
//                return NoContent();
//            }
//            if (result == LoginStatus.UserNotFound)
//            {
//                ModelState.AddModelError("", "User not found.");
//                Response.StatusCode = (int)HttpStatusCode.NotFound;
//                return PartialView("_Login", loginViewModel);
//            }
//            if (result == LoginStatus.WrongPassword)
//            {
//                ModelState.AddModelError("", "Wrong password.");
//                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
//                return PartialView("_Login", loginViewModel);
//            }
//            if (result == LoginStatus.IsBlocked)
//            {
//                ModelState.AddModelError("", "User is blocked.");
//                Response.StatusCode = (int)HttpStatusCode.Forbidden;
//                return PartialView("_Login", loginViewModel);
//            }
//            return NoContent();
//        }

//        public async Task<IActionResult> Logout()
//        {
//            await userManager.SignOutAsync();
//            return RedirectToAction("Index", "Home");
//        }

//        [HttpPost]
//        public async Task<IActionResult> BlockLogout()
//        {
//            if (User?.Identity?.Name == null)
//            {
//                return NoContent();
//            }
//            var username = User.Identity.Name;
//            var result = await userManager.GetByNameAsync(username);
//            if (result.IsBlocked)
//            {
//                await userManager.SignOutAsync();
//                ModelState.AddModelError("", "User is blocked.");
//                Response.StatusCode = (int)HttpStatusCode.Forbidden;
//                return Forbid();
//            }
//            else
//            {
//                return NoContent();
//            }

//        }

//        [HttpGet]
//        public IActionResult Register()
//        {
//            return PartialView("_Register");
//        }

//        [HttpPost]
//        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(registerViewModel);
//            }

//            if (registerViewModel.UserName == null ||
//                registerViewModel.Password == null ||
//                registerViewModel.Email == null)
//            {
//                return View();
//            }

//            var result = await userManager.CreateAsync(registerViewModel.UserName, registerViewModel.Password, registerViewModel.Email);

//            if (result)
//            {
//                await userManager.SignInAsync(registerViewModel.UserName, registerViewModel.Password);
//                return NoContent();
//            }
//            else
//            {
//                ModelState.AddModelError("", "User already exists.");
//                Response.StatusCode = (int)HttpStatusCode.Conflict;
//                return PartialView("_Register", registerViewModel);
//            }

//        }
//    }
//}

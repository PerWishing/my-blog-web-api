using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyBlog.Web.ViewModels.Accounts;
using System.Net;
using System.Net.Mime;
using MyBlog.Web.Models.Enums;
using MyBlog.Persistance.Repositories.UserRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using MyBlog.Persistance.Identity;
using MyBlog.Web.Extentions;
using MyBlog.Web.ViewModels.JwtToken;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MyBlog.Web.Controllers.Accounts
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly IConfiguration configuration;

        public AccountsController(IUserManager userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (loginViewModel.UserName == null || loginViewModel.Password == null)
            {
                return BadRequest();
            }

            var result = (LoginStatus)await userManager.CheckPasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password);

            if (result == LoginStatus.UserNotFound)
            {
                ModelState.AddModelError("", "User not found.");
                return NotFound(loginViewModel);
            }
            if (result == LoginStatus.WrongPassword)
            {
                ModelState.AddModelError("", "Wrong password.");
                return Unauthorized(loginViewModel);
            }
            if (result == LoginStatus.IsBlocked)
            {
                ModelState.AddModelError("", "User is blocked.");
                return Forbid();
            }

            var appUser = await userManager.GetIdentityUserByNameAsync(loginViewModel.UserName);

            var roles = await userManager.GetRolesAsync(loginViewModel.UserName);

            var claims = appUser.CreateClaims(roles);

            var tokenHandler = new JwtSecurityTokenHandler();

            var accessToken = tokenHandler.WriteToken(configuration.CreateToken(claims));

            var RefreshToken = configuration.GenerateRefreshToken();
            var RefreshTokenExpiryTime = configuration.GetRefreshTokenExpiryTime();

            var RefreshTokenRequest = new SetRefreshTokenRequest
            {
                Username = loginViewModel.UserName,
                RefreshToken = RefreshToken,
                RefreshTokenExpiryTime = RefreshTokenExpiryTime
            };

            await userManager.SetUserRefreshTokenAsync(RefreshTokenRequest);

            return Ok(new TokenModel { AccessToken = accessToken, RefreshToken = RefreshToken });
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (registerViewModel.UserName == null ||
                registerViewModel.Password == null ||
                registerViewModel.Email == null)
            {
                return BadRequest(registerViewModel);
            }

            var createResult = await userManager.CreateAsync(
                registerViewModel.UserName,
                registerViewModel.Password,
                registerViewModel.Email);

            if (!createResult)
            {
                ModelState.AddModelError("", "User already exists.");
                Response.StatusCode = (int)HttpStatusCode.Conflict;
                return Conflict();
            }

            return Ok();
        }

        [Route("RefreshToken")]
        [HttpPost]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            var accessToken = tokenModel.AccessToken;
            var refreshToken = tokenModel.RefreshToken;

            Regex rx = new Regex(@"\\[uU]([0-9A-F]{4})");
            refreshToken = rx.Replace(refreshToken!, match => ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());

            var principal = configuration.GetPrincipalFromExpiredToken(accessToken);

            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token.");
            }

            var username = principal.Identity!.Name;

            var user = await userManager.GetIdentityUserByNameAsync(username!);

            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (user.RefreshToken != refreshToken)
            {
                return BadRequest("Invalid refresh token.");
            }
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Conflict("Refresh token has expired.");
            }

            var newAccessToken = configuration.CreateToken(principal.Claims.ToList());
            var newRefreshToken = configuration.GenerateRefreshToken();

            await userManager.SetUserRefreshTokenAsync(new SetRefreshTokenRequest
            {
                Username = username!,
                RefreshToken = newRefreshToken
            });

            return Ok(new TokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            });
        }

        [Route("Revoke/{username}")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Revoke(string username)
        {
            var result = await userManager.RevokeUserRefreshTokenAsync(username);

            if (result)
                return Ok();
            else
                return BadRequest("Invalid user name");
        }
    }
}

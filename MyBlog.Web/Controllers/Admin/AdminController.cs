using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.UserRepository;

namespace MyBlog.Web.Controllers.Admin
{
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    [Produces("application/json")]
    public class AdminController : ControllerBase
    {
        private readonly IUserManager userManager;

        public AdminController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [Route("api/admin/block-user/{username}")]
        [HttpPut]
        public async Task<IActionResult> BlockUser(string username)
        {
            await userManager.BlockByNameAsync(username);

            return Ok();
        }

        [Route("api/admin/unblock-user/{username}")]
        [HttpPut]
        public async Task<IActionResult> UnblockUser(string username)
        {
            await userManager.UnblockByNameAsync(username);

            return Ok();
        }

        [Route("api/admin/give-admin/{username}")]
        [HttpPut]
        public async Task<IActionResult> GiveAdminRole(string username)
        {
            await userManager.GiveAdminRoleAsync(username);

            return Ok();
        }

        [Route("api/admin/delete-admin/{username}")]
        [HttpPut]
        public async Task<IActionResult> DeleteAdminRole(string username)
        {
            await userManager.DeleteAdminRoleAsync(username);

            return Ok();
        }
    }
}

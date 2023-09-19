using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Persistance.Repositories.UserRepository;

namespace MyBlog.Web.Controllers.Admin
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly IUserManager userManager;

        public AdminController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(string username)
        {
            await userManager.BlockByNameAsync(username);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(string username)
        {
            await userManager.UnblockByNameAsync(username);

            return NoContent();
        }
    }
}

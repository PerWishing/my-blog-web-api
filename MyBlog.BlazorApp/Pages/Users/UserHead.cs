using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.User;
using MyBlog.BlazorApp.Services.User;

namespace MyBlog.BlazorApp.Pages.Users
{
    public partial class UserHead
    {
        [Inject]
        private IUserService userService { get; set; } = null!;

        [Parameter]
        public string? Username { get; set; }

        public UserVm _userVm = new UserVm();

        protected override async Task OnParametersSetAsync()
        {
            _userVm = await userService.GetUserAvatarAsync(Username!);
        }
    }
}

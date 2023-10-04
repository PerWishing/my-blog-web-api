using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.User;
using MyBlog.BlazorApp.Services.User;

namespace MyBlog.BlazorApp.Pages.Users
{
    public partial class UserHeadBig
    {
        [Inject]
        private IUserService userService { get; set; } = null!;

        [Parameter]
        public string? Username { get; set; }

        public UserDto _userDto = new UserDto();

        protected override async Task OnParametersSetAsync()
        {
            _userDto = await userService.GetUserByNameAsync(Username!);
        }
    }
}

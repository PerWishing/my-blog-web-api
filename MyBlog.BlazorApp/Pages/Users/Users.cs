using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models;
using MyBlog.BlazorApp.Services.User.UserService;

namespace MyBlog.BlazorApp.Pages.Users
{
    public partial class Users
    {
        [Inject]
        private IUserService userService { get; set; } = null!;

        public UsersPageDto _usersPage = new UsersPageDto(); 

        protected override async Task OnInitializedAsync()
        {
            var apiUsersPage = await userService.GetUsersAsync();

            if(apiUsersPage != null)
            {
                _usersPage = apiUsersPage;
            }
        }
    }
}

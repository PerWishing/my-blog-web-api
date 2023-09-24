using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.User;
using MyBlog.BlazorApp.Services.User.UserService;

namespace MyBlog.BlazorApp.Pages.Users
{
    public partial class Users
    {
        [Inject]
        private IUserService userService { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Parameter]
        public int Page { get; set; }
        [Parameter]
        public string Search { get; set; }

        public UsersPageDto _usersPage = new UsersPageDto();

        protected override async Task OnParametersSetAsync()
        {
            if (Page == 0) { Page = 1; }
            var apiUsersPage = await userService.GetUsersAsync(Page, Search);

            if (apiUsersPage != null)
            {
                _usersPage = apiUsersPage;
            }
        }

        async Task OnNextPage()
        {
            Page += 1;
            if (string.IsNullOrEmpty(Search))
                NavigationManager.NavigateTo("users/" + Page);
            else
                NavigationManager.NavigateTo("users/" + Search + "/" + Page);
        }

        async Task OnPrevPage()
        {
            Page -= 1;
            if (string.IsNullOrEmpty(Search))
                NavigationManager.NavigateTo("users/" + Page);
            else
                NavigationManager.NavigateTo("users/" + Search + "/" + Page);
        }

        async Task PerformSearch()
        {
            NavigationManager.NavigateTo("users/" + Search + "/1");
        }
    }
}

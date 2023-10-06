using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.User;
using MyBlog.BlazorApp.Services.User;

namespace MyBlog.BlazorApp.Pages.Users
{
    public partial class ReadersOrFollowed
    {
        [Inject]
        private IUserService userService { get; set; } = null!;
        [CascadingParameter(Name = "Username")]
        public string? Username { get; set; }
        public UsersPageVm _usersPage = new UsersPageVm();
        [CascadingParameter(Name = "IsReaders")]
        public bool IsReaders { get; set; }
        [CascadingParameter(Name = "IsFollowedUsers")]
        public bool IsFollowedUsers { get; set; }
        [Parameter]
        public int Page { get; set; }
        [Parameter]
        public string Search { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await ResetParametersAsync();
            if (Page == 0) { Page = 1; }

            UsersPageVm? apiUsersPage = new UsersPageVm();
            if (IsReaders)
            {
                apiUsersPage = await userService.GetReadersAsync(Username!, Page, Search);

                if (apiUsersPage != null)
                {
                    _usersPage = apiUsersPage;
                }
            }
            if (IsFollowedUsers)
            {
                apiUsersPage = await userService.GetFollowedUsersAsync(Username!, Page, Search);

                if (apiUsersPage != null)
                {
                    _usersPage = apiUsersPage;
                }
            }
            StateHasChanged();
            await base.OnParametersSetAsync();

        }
        async Task ResetParametersAsync()
        {
            _usersPage = new UsersPageVm();
        }

        public async Task RefreshComponentAsync()
        {
            await OnParametersSetAsync();
        }
        async Task OnNextPage()
        {
            Page += 1;
            await RefreshComponentAsync();
        }
        async Task OnPrevPage()
        {
            Page -= 1;
            await RefreshComponentAsync();
        }

        async Task PerformSearch()
        {
            Page = 1;
            await RefreshComponentAsync();
        }
    }
}

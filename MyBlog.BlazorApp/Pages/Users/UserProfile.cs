using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.User;
using MyBlog.BlazorApp.Services.User;

namespace MyBlog.BlazorApp.Pages.Users
{
    public partial class UserProfile
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject]
        private IUserService userService { get; set; } = null!;

        [Parameter]
        public string? Username { get; set; } 
        [Parameter]
        public bool IsAuth { get; set; }
        [Parameter]
        public bool IsAdmin { get; set; }

        public bool IsBlocked { get; set; }
        [Parameter]
        public bool IsCurrentUser { get; set; }
        [Parameter]
        public bool IsReader { get; set; }
        [Parameter]
        public bool IsFollowed { get; set; }
        [Parameter]
        public bool IsSaved { get; set; }
        [Parameter]
        public bool IsEditToggled { get; set; }
        [Parameter]
        public bool GetChild { get; set; } = true;

        public UserVm _userVm = new UserVm();

        public IsReaderOrFollowedVm? IsReaderOrFollowed = new IsReaderOrFollowedVm();

        protected ReadersOrFollowed? childReaders;

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
        }
        protected override async Task OnParametersSetAsync()
        {
            await ResetParametersAsync();

            var result = await AuthStateProvider.GetAuthenticationStateAsync();
            IsAuth = result.User.Identity!.IsAuthenticated;
            IsAdmin = result.User.IsInRole("Admin");
            await Console.Out.WriteLineAsync("IsAdmin: "+IsAdmin);
            if (Username == null)
            {
                if (!IsAuth)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    Username = result.User.Identity.Name;
                }
            }

            if (result.User.Identity!.Name == Username)
            {
                IsCurrentUser = true;
            }

            var apiUser = await userService.GetUserByNameAsync(Username!);

            if (apiUser != null)
            {
                _userVm = apiUser;
            }

            IsBlocked = _userVm.isBlocked;

            if (IsAuth && !IsCurrentUser)
            {
                IsReaderOrFollowed = await userService.IsReaderOrFollowedAsync(Username!);
                if(IsReaderOrFollowed != null)
                {
                    IsReader = IsReaderOrFollowed.IsReader;
                    IsFollowed = IsReaderOrFollowed.IsFollowed;
                }
            }

            StateHasChanged();

            await base.OnParametersSetAsync();

        }
        async Task OnToggleEdit()
        {
            if (IsEditToggled)
            {
                await userService.EditUserAsync(_userVm);

                IsEditToggled = false;
            }
            else
            {
                IsEditToggled = true;
            }
        }

        async Task OnFollowToggle()
        {
            if (IsFollowed)
            {
                await userService.StopReadAsync(Username!);
                
                IsFollowed = false;

                childReaders.RefreshComponentAsync();
            }
            else
            {
                await userService.StartReadAsync(Username!);

                IsFollowed = true;

                childReaders.RefreshComponentAsync();
            }
        }

        async Task OnToggleBlock()
        {
            if (IsBlocked)
            {
                await userService.UnblockUserAsync(_userVm.UserName);

                await RefreshComponentAsync();
            }
            else
            {
                await userService.BlockUserAsync(_userVm.UserName);

                await RefreshComponentAsync();
            }
        }

        async Task OnToggleGiveAdmin()
        {
            if (_userVm.isAdmin)
            {
                await userService.DeleteAdminAsync(_userVm.UserName);

                await RefreshComponentAsync();
            }
            else
            {
                await userService.GiveAdminAsync(_userVm.UserName);

                await RefreshComponentAsync();
            }
        }

        async Task ResetParametersAsync()
        {
            IsAuth = false;
            IsAdmin = false;
            IsBlocked = false;
            IsCurrentUser = false;
            IsFollowed = false;
            IsReader = false;
            IsEditToggled = false;
            IsSaved = false;
            GetChild = true;
        }
        async Task RefreshComponentAsync()
        {
            await OnParametersSetAsync();
            StateHasChanged();
        }
    }
}

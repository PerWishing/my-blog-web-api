using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.User;
using MyBlog.BlazorApp.Services.User.UserService;

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
        public bool IsCurrentUser { get; set; }
        [Parameter]
        public bool IsReader { get; set; }
        [Parameter]
        public bool IsFollowed { get; set; }
        [Parameter]
        public bool IsEditToggled { get; set; }
        [Parameter]
        public bool GetChild { get; set; } = true;

        public UserDto _userDto = new UserDto();

        public IsReaderOrFollowedDto? IsReaderOrFollowed = new IsReaderOrFollowedDto();

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
                _userDto = apiUser;
            }

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
                await userService.EditUserAsync(_userDto);

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

        async Task ResetParametersAsync()
        {
            IsAuth = false;
            IsCurrentUser = false;
            IsFollowed = false;
            IsReader = false;
            IsEditToggled = false;
            GetChild = true;
        }
    }
}

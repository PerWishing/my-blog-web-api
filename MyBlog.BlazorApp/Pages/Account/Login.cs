using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models;
using MyBlog.BlazorApp.Services.User.UserService;

namespace MyBlog.BlazorApp.Pages.Account
{
    public partial class Login
    {
        [Inject]
        private IUserService userService { get; set; } = null!;
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        LoginDto login = new LoginDto();
        List<string> ErrorMessages = new List<string>();

        async Task HandleLogin()
        {
            var error = await userService.SignInAsync(login);
            if (error != null)
            {
                ErrorMessages.Add(error);
            }
            await AuthStateProvider.GetAuthenticationStateAsync();
        }
        protected async override Task OnInitializedAsync()
        {
            var result = await AuthStateProvider.GetAuthenticationStateAsync();
            if (result.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}

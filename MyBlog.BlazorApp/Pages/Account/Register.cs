using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models;
using MyBlog.BlazorApp.Services.User.UserService;

namespace MyBlog.BlazorApp.Pages.Account
{
    public partial class Register
    {
        [Inject]
        private IUserService userService { get; set; } = null!;
        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        RegisterDto register = new RegisterDto();

        async Task HandleRegister()
        {
            await userService.RegisterAsync(register);
            await AuthStateProvider.GetAuthenticationStateAsync();
            NavigationManager.NavigateTo("/");
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

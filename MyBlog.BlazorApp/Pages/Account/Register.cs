using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models;
using MyBlog.BlazorApp.Services.User;

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
        List<string> ErrorMessages = new List<string>();


        async Task HandleRegister()
        {
            var error = await userService.RegisterAsync(register);
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

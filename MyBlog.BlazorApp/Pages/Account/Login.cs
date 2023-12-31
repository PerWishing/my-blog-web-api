﻿using Microsoft.AspNetCore.Components;
using MyBlog.BlazorApp.Models.User;
using MyBlog.BlazorApp.Services.User;

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

        LoginVm login = new LoginVm();
        List<string> ErrorMessages = new List<string>();

        async Task HandleLogin()
        {
            var error = await userService.SignInAsync(login);
            if (error != null)
            {
                ErrorMessages.Add(error);
            }
            await AuthStateProvider.GetAuthenticationStateAsync();
            NavigationManager.NavigateTo("/user");
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

using MyBlog.BlazorApp.Models;
using MyBlog.BlazorApp.Pages.Account;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Net;

namespace MyBlog.BlazorApp.Services.User.UserService
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
        }
        public async Task<UsersPageDto?> GetUsersAsync()
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync("api/users");

                var usersPage = await JsonSerializer.DeserializeAsync<UsersPageDto>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return usersPage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> SignInAsync(LoginDto login)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PostAsync("api/Accounts/Login", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseBody = await apiResponse.Content.ReadAsStreamAsync();

                    var jwtTokenDto = await JsonSerializer.DeserializeAsync<JwtTokenDto>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    await localStorage.SetItemAsync("AccessToken", jwtTokenDto!.AccessToken);
                    await localStorage.SetItemAsync("RefreshToken", jwtTokenDto.RefreshToken);

                    return null;
                }
                if (apiResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return "User not found.";
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

        }
        public async Task<bool> SignOutAsync(string username)
        {
            var apiResponse = await httpClient.PostAsync($"api/Accounts/Revoke/{username}",
                new StringContent(username, Encoding.UTF8));

            await localStorage.RemoveItemAsync("AccessToken");
            await localStorage.RemoveItemAsync("RefreshToken");
            return true;
        }
        public async Task<bool> RegisterAsync(RegisterDto register)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(register), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PostAsync("api/Accounts/Register", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    await SignInAsync(new LoginDto { 
                        UserName = register.UserName, 
                        Password = register.Password
                    });
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<JwtTokenDto?> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            try
            {
                var tokenRequest = new JwtTokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
                var itemJson = new StringContent(JsonSerializer.Serialize(tokenRequest), Encoding.UTF8, "application/json");

                var apiResponse = await httpClient.PostAsync("api/Accounts/RefreshToken", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseBody = await apiResponse.Content.ReadAsStreamAsync();

                    var jwtTokenDto = await JsonSerializer.DeserializeAsync<JwtTokenDto>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                    return jwtTokenDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}

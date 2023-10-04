using MyBlog.BlazorApp.Models;
using MyBlog.BlazorApp.Pages.Account;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Collections;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using MyBlog.BlazorApp.Models.User;

namespace MyBlog.BlazorApp.Services.User
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

        public async Task<string?> UploadAvatarAsync(byte[] blob)
        {
            try
            {
                var multipartContent = new MultipartFormDataContent();
                var byteArrayContent = new ByteArrayContent(blob);
                multipartContent.Add(byteArrayContent, "blob", "img.png");
                var apiResponse = await httpClient.PostAsync("api/avatars/upload", multipartContent);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDto?> GetUserByNameAsync(string username)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/user/{username}");

                var userDto = await JsonSerializer.DeserializeAsync<UserDto>(apiResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                var avatarBase64Str = await httpClient.GetStringAsync($"api/avatars/{username}");
                avatarBase64Str = avatarBase64Str.Replace("\"", "");
                userDto!.AvatarSrc = "data:image/png;base64," + avatarBase64Str;

                return userDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<UsersPageDto?> GetUsersAsync(int page = 1, string? search = null)
        {
            try
            {
                Stream apiResponse = null;
                if (string.IsNullOrEmpty(search))
                {
                    apiResponse = await httpClient.GetStreamAsync($"api/users/{page}");
                }
                else
                {
                    apiResponse = await httpClient.GetStreamAsync($"api/users/{search}/{page}");
                }

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

        public async Task<UserDto?> GetUserAvatarAsync(string username)
        {
            try
            {
                var userDto = new UserDto { UserName = username };

                var avatarBase64Str = await httpClient.GetStringAsync($"api/avatars/{username}");
                avatarBase64Str = avatarBase64Str.Replace("\"", "");
                userDto!.AvatarSrc = "data:image/png;base64," + avatarBase64Str;

                return userDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> EditUserAsync(UserDto userDto)
        {
            try
            {
                var editUserDto = new EditUserDto
                {
                    UserName = userDto.UserName,
                    AboutMyself = userDto.AboutMyself
                };

                var itemJson = new StringContent(JsonSerializer.Serialize(editUserDto), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PutAsync("api/user/edit", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                if (apiResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return "You can't edit this user.";
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<IsReaderOrFollowedDto?> IsReaderOrFollowedAsync(string username)
        {
            try
            {
                var apiResponse = await httpClient.GetStreamAsync($"api/user/reader-or-followed/{username}");

                var isReaderOrFollowedDto = await JsonSerializer.DeserializeAsync<IsReaderOrFollowedDto>(apiResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return isReaderOrFollowedDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<UsersPageDto?> GetReadersAsync(string username, int page = 1, string? search = null)
        {
            try
            {
                Stream apiResponse = null;

                if (string.IsNullOrEmpty(search))
                {
                    apiResponse = await httpClient.GetStreamAsync($"api/readers/{username}/{page}");
                }
                else
                {
                    apiResponse = await httpClient.GetStreamAsync($"api/readers/{username}/{search}/{page}");
                }
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

        public async Task<UsersPageDto?> GetFollowedUsersAsync(string username, int page = 1, string? search = null)
        {
            try
            {
                Stream apiResponse = null;

                if (string.IsNullOrEmpty(search))
                {
                    apiResponse = await httpClient.GetStreamAsync($"api/followed/{username}/{page}");
                }
                else
                {
                    apiResponse = await httpClient.GetStreamAsync($"api/followed/{username}/{search}/{page}");
                }

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

        public async Task<string?> StartReadAsync(string username)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(username), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PostAsync($"api/user/start-read/{username}", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                return "Wrong user name.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> StopReadAsync(string username)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(username), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PostAsync($"api/user/stop-read/{username}", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                return "Wrong user name.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> BlockUserAsync(string username)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(username), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PutAsync($"api/admin/block-user/{username}", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                if (apiResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return "You can't block this user.";
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> UnblockUserAsync(string username)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(username), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PutAsync($"api/admin/unblock-user/{username}", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                if (apiResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return "You can't unblock this user.";
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> GiveAdminAsync(string username)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(username), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PutAsync($"api/admin/give-admin/{username}", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                if (apiResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return "You can't give admin role to this user.";
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> DeleteAdminAsync(string username)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(username), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PutAsync($"api/admin/delete-admin/{username}", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return null;
                }
                if (apiResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return "You can't delete admin role from this user.";
                }
                return null;
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
                if (apiResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return "Wrong password.";
                }
                if (apiResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return "User is blocked.";
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
        public async Task<string?> RegisterAsync(RegisterDto register)
        {
            try
            {
                var itemJson = new StringContent(JsonSerializer.Serialize(register), Encoding.UTF8, "application/json");
                var apiResponse = await httpClient.PostAsync("api/Accounts/Register", itemJson);

                if (apiResponse.IsSuccessStatusCode)
                {
                    await SignInAsync(new LoginDto
                    {
                        UserName = register.UserName,
                        Password = register.Password
                    });
                    return null;
                }
                if (apiResponse.StatusCode == HttpStatusCode.Conflict)
                {
                    return "User already exists.";
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
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

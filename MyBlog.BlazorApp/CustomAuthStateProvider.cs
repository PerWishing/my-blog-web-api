using MyBlog.BlazorApp.Services.User;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace MyBlog.BlazorApp
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorage;
        private readonly HttpClient http;
        private readonly IUserService userService;

        public CustomAuthStateProvider(ILocalStorageService localStorage,
            HttpClient http,
            IUserService userService)
        {
            this.localStorage = localStorage;
            this.http = http;
            this.userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await localStorage.GetItemAsStringAsync("AccessToken");

            var identity = new ClaimsIdentity();
            http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                var claims = ParseClaimsFromJwt(token);
                var exp = claims.First(c => c.Type == "exp");
                if (exp != null)
                {
                    DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    int seconds = int.Parse(exp.Value.ToString()!);
                    DateTime expTime = startDate.AddSeconds(seconds-600);
                    if (expTime <= DateTime.UtcNow)
                    {
                        Console.WriteLine("EXPIRED!!!!!!!!");
                        Console.WriteLine(expTime);
                        Console.WriteLine(DateTime.UtcNow);
                        var refreshJwtResult = await RefreshJwtToken();
                        if (refreshJwtResult)
                        {
                            token = await localStorage.GetItemAsStringAsync("AccessToken");
                            claims = ParseClaimsFromJwt(token);
                        }
                        else
                        {
                            await localStorage.RemoveItemAsync("AccessToken");
                            await localStorage.RemoveItemAsync("RefreshToken");
                            claims = new List<Claim>();
                        }
                    }
                }
                identity = new ClaimsIdentity(claims, "jwt");
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        public async Task<bool> RefreshJwtToken()
        {
            var accessToken = await localStorage.GetItemAsStringAsync("AccessToken");
            var refreshToken = await localStorage.GetItemAsStringAsync("RefreshToken");

            if(accessToken == null || refreshToken == null) {
                await Console.Out.WriteLineAsync("RefreshJwtToken FAILED");
                return false; }

            accessToken = accessToken.Replace("\"", "");
            refreshToken = refreshToken.Replace("\"", "");

            var newJwtTokenVm = await userService.RefreshTokenAsync(accessToken, refreshToken);
            if(newJwtTokenVm == null) { return false; }
            await localStorage.SetItemAsStringAsync("AccessToken", newJwtTokenVm.AccessToken);
            await localStorage.SetItemAsStringAsync("RefreshToken", newJwtTokenVm.RefreshToken);
            return true;
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}

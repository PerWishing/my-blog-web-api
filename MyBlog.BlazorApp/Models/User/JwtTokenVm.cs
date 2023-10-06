namespace MyBlog.BlazorApp.Models.User
{
    public class JwtTokenVm
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}

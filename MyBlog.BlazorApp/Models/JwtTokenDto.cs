namespace MyBlog.BlazorApp.Models
{
    public class JwtTokenDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}

namespace MyBlog.BlazorApp.Models.User
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;
        public string? AboutMyself { get; set; }
        public bool isBlocked { get; set; }
        public bool isAdmin { get; set; }
        public string? AvatarSrc { get; set; }
    }
}

namespace MyBlog.BlazorApp.Models
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;
        public string? AvatarName { get; set; }
        public string? AboutMyself { get; set; }
        public bool isFollowedByYou { get; set; }
        public bool isReader { get; set; }
        public bool isBlocked { get; set; }
    }
}

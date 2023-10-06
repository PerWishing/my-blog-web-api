namespace MyBlog.Web.Dto.User
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;
        public string? AboutMyself { get; set; }
        public bool isBlocked { get; set; }
        public bool isAdmin { get; set; }
    }
}

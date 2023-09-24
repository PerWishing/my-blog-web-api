namespace MyBlog.BlazorApp.Models.User
{
    public class UsersPageDto
    {
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; }
        public List<UserDto>? Users { get; set; }
    }
}

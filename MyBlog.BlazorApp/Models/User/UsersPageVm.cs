namespace MyBlog.BlazorApp.Models.User
{
    public class UsersPageVm
    {
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; }
        public List<UserVm>? Users { get; set; }
    }
}

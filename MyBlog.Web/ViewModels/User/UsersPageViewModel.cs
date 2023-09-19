namespace MyBlog.Web.ViewModels.User
{
    public class UsersPageViewModel
    {
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; }
        public List<UserViewModel>? Users { get; set; }
    }
}

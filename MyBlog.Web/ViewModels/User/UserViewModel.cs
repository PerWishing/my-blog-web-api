using MyBlog.Web.Models.Enums;

namespace MyBlog.Web.ViewModels.User
{
    public class UserViewModel
    {
        public string UserName { get; set; } = null!;
        public string? AboutMyself { get; set; }
        public bool isBlocked { get; set; }
        public bool isAdmin { get; set; }
    }
}

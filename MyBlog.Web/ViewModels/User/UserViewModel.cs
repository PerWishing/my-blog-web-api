using MyBlog.Web.Models.Enums;

namespace MyBlog.Web.ViewModels.User
{
    public class UserViewModel
    {
        public string UserName { get; set; } = null!;
        public string? AvatarName { get; set; }
        public string? AboutMyself { get; set; }
        public bool isFollowedByYou { get; set; }
        public bool isReader { get; set; }
        public bool isBlocked { get; set; }
    }
}

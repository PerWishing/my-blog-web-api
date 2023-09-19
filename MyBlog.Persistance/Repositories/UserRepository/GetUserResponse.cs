namespace MyBlog.Persistance.Repositories.UserRepository
{
    public class GetUserResponse
    {
        public string UserName { get; set; } = null!;
        public string? AvatarName { get; set; }
        public string? AboutMyself { get; set; }
        public bool IsBlocked { get; set; }
    }
}

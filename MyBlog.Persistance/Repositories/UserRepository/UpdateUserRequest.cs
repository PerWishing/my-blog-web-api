namespace MyBlog.Persistance.Repositories.UserRepository
{
    public class UpdateUserRequest
    {
        public string UserName { get; set; } = null!;
        public string? AboutMyself { get; set; }
    }
}

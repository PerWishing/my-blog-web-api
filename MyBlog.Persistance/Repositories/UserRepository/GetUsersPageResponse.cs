namespace MyBlog.Persistance.Repositories.UserRepository
{
    public class GetUsersPageResponse
    {
        public int PageCount { get; set; }
        public IList<GetUserResponse>? Users { get; set; }
    }
}

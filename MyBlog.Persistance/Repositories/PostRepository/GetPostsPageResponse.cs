namespace MyBlog.Persistance.Repositories.PostRepository
{
    public class GetPostsPageResponse
    {
        public int PageCount { get; set; }
        public IList<GetPostResponse>? Posts { get; set; }
    }
}

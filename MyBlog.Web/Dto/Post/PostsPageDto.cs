namespace MyBlog.Web.Dto.Post
{
    public class PostsPageDto
    {
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; }

        public List<PostDto>? Posts { get; set; }
    }
}

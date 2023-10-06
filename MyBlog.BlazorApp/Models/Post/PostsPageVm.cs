namespace MyBlog.BlazorApp.Models.Post
{
    public class PostsPageVm
    {
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; }

        public List<PostVm>? Posts { get; set; }
    }
}

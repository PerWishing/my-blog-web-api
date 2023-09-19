namespace MyBlog.Web.ViewModels.Post
{
    public class PostsPageViewModel
    {
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; }

        public List<PostViewModel>? Posts { get; set; }
    }
}

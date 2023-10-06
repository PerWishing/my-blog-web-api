namespace MyBlog.BlazorApp.Models.Comment
{
    public class CreateCommentVm
    {
        public int PostId { get; set; }
        public string Text { get; set; } = null!;
        public string AuthorsName { get; set; } = null!;
    }
}

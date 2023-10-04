namespace MyBlog.BlazorApp.Models.Comments
{
    public class CreateCommentDto
    {
        public int PostId { get; set; }
        public string Text { get; set; } = null!;
        public string AuthorsName { get; set; } = null!;
    }
}

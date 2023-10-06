namespace MyBlog.BlazorApp.Models.Comment
{
    public class CommentVm
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; } = null!;
        public string AuthorsName { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool IsLiked { get; set; }
        public int LikesCount { get; set; }
    }
}

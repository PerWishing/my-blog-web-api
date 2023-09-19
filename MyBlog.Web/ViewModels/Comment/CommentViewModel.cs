namespace MyBlog.Web.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; } = null!;
        public string AuthorsName { get; set; } = null!;
        public string PostAuthorsName { get; set; } = null!;
        public DateTime Date { get; set; }
        public int LikesCount { get; set; }
        public bool IsLiked { get; set; }
    }
}

namespace MyBlog.Web.Dto.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; } = null!;
        public string AuthorsName { get; set; } = null!;
        public string Date { get; set; }
        public int LikesCount { get; set; }
    }
}

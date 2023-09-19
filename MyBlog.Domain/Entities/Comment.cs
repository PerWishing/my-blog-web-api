namespace MyBlog.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public UserProfile Author { get; set; } = null!;
        public Post Post { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}

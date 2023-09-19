namespace MyBlog.Domain.Entities
{
    public class PostImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = null!;
        public Post Post { get; set; } = null!;
    }
}

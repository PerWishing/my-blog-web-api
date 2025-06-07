using MyBlog.Domain.Entities.Summarizations;

namespace MyBlog.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime PublishDate { get; set; } 
        public UserProfile Author { get; set; } = null!;
        public IEnumerable<PostImage>? Images { get; set; }

        public ICollection<Summarization>? Summarizations { get; set; }
    }
}

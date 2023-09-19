namespace MyBlog.Domain.Entities
{
    public class Readers
    {
        public string ReaderId { get; set; } = null!;
        public UserProfile Reader { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public UserProfile User { get; set; } = null!;

    }
}

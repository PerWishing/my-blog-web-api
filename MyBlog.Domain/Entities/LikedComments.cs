namespace MyBlog.Domain.Entities
{
    public class LikedComments
    {
        public string UserProfileId { get; set; } = null!;
        public UserProfile UserProfile { get; set; } = null!;

        public int CommentId { get; set; }
        public Comment Comment { get; set; } = null!;
    }
}

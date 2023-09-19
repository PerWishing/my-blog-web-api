namespace MyBlog.Domain.Entities
{
    public class SavedPosts
    {
        public string UserProfileId { get; set; } = null!;
        public UserProfile UserProfile { get; set; } = null!;

        public int PostsId { get; set; }
        public Post Post { get; set; } = null!;
    }
}

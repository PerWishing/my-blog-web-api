namespace MyBlog.Domain.Entities
{
    public class UserAvatar
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = null!;
        public DateTime UploadDate { get; set; }
        public string UserProfileId { get; set; } = null!;
        public UserProfile UserProfile { get; set; } = null!;
    }
}

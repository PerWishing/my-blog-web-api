namespace MyBlog.Web.Dto.Image
{
    public class AvatarDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; } = null!;
        public DateTime UploadDate { get; set; }
        public string Username { get; set; } = null!;
        public IFormFile ImageFile { get; set; } = null!;
    }
}

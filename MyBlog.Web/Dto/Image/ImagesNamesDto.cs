namespace MyBlog.Web.Dto.Image
{
    public class ImagesNamesDto
    {
        public int PostId { get; set; }
        public IEnumerable<string>? ImagesNames { get; set;}
    }
}

namespace MyBlog.Web.ViewModels.Image
{
    public class ImagesNamesViewModel
    {
        public int PostId { get; set; }
        public IEnumerable<string>? ImagesNames { get; set;}
    }
}

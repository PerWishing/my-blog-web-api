using System.ComponentModel.DataAnnotations;

namespace MyBlog.Web.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title from 6 to 50 symbols.", MinimumLength = 6)]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Text is required.")]
        [StringLength(1000, ErrorMessage = "Minimum length of text is 10.", MinimumLength = 10)]
        public string Text { get; set; } = null!;
        public DateTime PublishDate { get; set; }
        public string AuthorsName { get; set; } = null!;
        public bool IsSaved { get; set; }
        public int SavesCount { get; set; }
        public IEnumerable<string>? Images64s { get; set; }
    }
}

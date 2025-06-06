using System.ComponentModel.DataAnnotations;

namespace MyBlog.Web.Dto.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title from 6 to 50 symbols.", MinimumLength = 6)]
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime PublishDate { get; set; }
        public string AuthorsName { get; set; } = null!;
        public bool IsSaved { get; set; }
        public int SavesCount { get; set; }
        public IEnumerable<string>? Images64s { get; set; }
        
        public string? SummarizedText { get; set; }
        public string? InputFileName { get; set; }
        public string? OutputFileName { get; set; }
    }
}

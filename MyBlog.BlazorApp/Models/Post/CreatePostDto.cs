using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.BlazorApp.Models.Post
{
    public class CreatePostDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title from 6 to 50 symbols.", MinimumLength = 6)]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Text is required.")]
        [StringLength(1000, ErrorMessage = "Minimum length of text is 10.", MinimumLength = 10)]
        public string Text { get; set; } = null!;
        public string? AuthorsName { get; set; }
    }
}

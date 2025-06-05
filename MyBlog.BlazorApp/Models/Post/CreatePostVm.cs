using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.BlazorApp.Models.Post
{
    public class CreatePostVm
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title from 6 to 50 symbols.", MinimumLength = 6)]
        public string Title { get; set; } = null!;
        public string? Text { get; set; } = null!;
        public string? AuthorsName { get; set; }
    }
}

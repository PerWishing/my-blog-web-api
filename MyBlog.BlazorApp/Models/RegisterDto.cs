using System.ComponentModel.DataAnnotations;

namespace MyBlog.BlazorApp.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Login is required.")]
        [StringLength(100, ErrorMessage = "Minimum length is 3.", MinimumLength = 3)]
        [RegularExpression(@"^(?!.*([Aa][Dd][Mm][Ii][Nn]|\s)).*$", ErrorMessage = "Login can't contain 'admin' word.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Minimum length is 6.", MinimumLength = 6)]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; }
    }
}

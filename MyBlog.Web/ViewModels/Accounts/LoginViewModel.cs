using System.ComponentModel.DataAnnotations;

namespace MyBlog.Web.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login is required.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Minimum length is 6.", MinimumLength = 6)]
        public string? Password { get; set; }
    }
}

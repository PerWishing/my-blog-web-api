namespace MyBlog.BlazorApp.Models.Summarizations;

public class CreateSumVm
{
    public int PostId { get; set; }
    public string? InputText { get; set; }

    public string? AuthorsName { get; set; }
}
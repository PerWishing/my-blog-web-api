namespace MyBlog.BlazorApp.Models.Summarizations;

public class SumVm
{
    public int SumId { get; set; }
    public int PostId { get; set; }
    public bool IsFile { get; set; }
    public string CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    
    public string? InputText { get; set; }
    public string? OutputText { get; set; }
    
    public string? InputFileName { get; set; }
    public string? OutputFileName { get; set; }
}
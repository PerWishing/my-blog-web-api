namespace MyBlog.Domain.Entities.Summarizations.Parameters;

public class CreateFileSummarizationParams
{
    public required string InputFilePath { get; set; }
    public required string OutputSummarizedFilePath { get; set; }
    public required UserProfile CreatedBy { get; set; }
}
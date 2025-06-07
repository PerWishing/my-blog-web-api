namespace MyBlog.Domain.Entities.Summarizations.Parameters;

public class CreateFileSummarizationParams
{
    public required int PostId { get; set; }
    public required Post Post { get; set; }
    public required string InputFilePath { get; set; }
    public required string InputFileName { get; set; }
    public required string OutputSummarizedFilePath { get; set; }
    public required UserProfile CreatedBy { get; set; }
}
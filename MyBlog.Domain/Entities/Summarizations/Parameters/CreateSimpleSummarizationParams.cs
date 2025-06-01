namespace MyBlog.Domain.Entities.Summarizations.Parameters;

public class CreateSimpleSummarizationParams
{
    public required string InputText { get; set; }
    public required string TopicText { get; set; }
    public required string OutputSummarizedText { get; set; }
    public required int CreatedBy { get; set; }
}
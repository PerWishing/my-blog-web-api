namespace MyBlog.Domain.Entities.Summarizations.Parameters;

public class CreateSimpleSummarizationParams
{
    public required int PostId { get; set; }
    public required Post Post { get; set; }
    public required string InputText { get; set; }
    public required string TopicText { get; set; }
    public required string OutputSummarizedText { get; set; }
    public required UserProfile CreatedBy { get; set; }
}
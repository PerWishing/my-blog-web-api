namespace MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;

public class SummarizationSimpleResult
{
    public required string InputText { get; set; }
    public required string TopicText { get; set; }
    public required string OutputSummarizedText { get; set; }
    public required DateTime CreatedAt { get; set; }
}
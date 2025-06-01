using MyBlog.Domain.Entities.Summarizations.Enums;
using MyBlog.Domain.Entities.Summarizations.Parameters;

namespace MyBlog.Domain.Entities.Summarizations;

public class Summarization
{
#pragma warning disable CS8618, CS9264
    private Summarization() { }
#pragma warning restore CS8618, CS9264

    public Summarization(CreateSimpleSummarizationParams parameters)
    {
        InputText = parameters.InputText;
        OutputSummarizedText = parameters.OutputSummarizedText;
        
        Author = parameters.CreatedBy;
        
        CreatedAt = DateTime.Now;

        SummarizationType = SummarizationType.Simple;
    }
    
    public int Id { get; private set; }

    public string? InputText { get; private set; }
    public string? OutputSummarizedText { get; private set; }
    
    public int? InputFileId { get; private set; }
    public int? OutputSummarizedFileId { get; private set; }

    public string? TopicText { get; private set; }
    
    public UserProfile Author { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public SummarizationType SummarizationType { get; set; }
}
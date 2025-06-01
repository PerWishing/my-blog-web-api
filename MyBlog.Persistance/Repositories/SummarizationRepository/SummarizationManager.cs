using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Entities.Summarizations;
using MyBlog.Domain.Entities.Summarizations.Parameters;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.ExternalFeatures.SummarizationModelApis;

namespace MyBlog.Persistance.Repositories.SummarizationRepository;

public class SummarizationManager
{
    private readonly ApplicationDbContext context;
    private readonly SummarizationModelHttpClient httpClient;

    public SummarizationManager(
        ApplicationDbContext context,
        SummarizationModelHttpClient httpClient)
    {
        this.context = context;
        this.httpClient = httpClient;
    }

    public async Task<string> DoSummarization(string inputText)
    {
        var summarizationExisting = await context.Summarizations
            .AsNoTracking()
            .Where(s => s.InputText == inputText)
            .SingleOrDefaultAsync();

        if (summarizationExisting != null)
        {
            return summarizationExisting.OutputSummarizedText ?? "";
        }
        
        var result = await httpClient.GetSummarizationFromApi(inputText);

        var newSum = new Summarization(new CreateSimpleSummarizationParams
        {
            InputText = inputText,
            TopicText = result.Topic,
            OutputSummarizedText = result.Summarized,
            CreatedBy = 0,
        });
        
        context.Summarizations.Add(newSum);
        
        return "";
    }
}
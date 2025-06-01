using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Entities.Summarizations;
using MyBlog.Domain.Entities.Summarizations.Parameters;
using MyBlog.Domain.Exceptions;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.ExternalFeatures.SummarizationModelApis;
using MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;
using MyBlog.Persistance.Repositories.UserRepository;

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

    public async Task<SummarizationSimpleResult> DoSimpleSummarization(
        string inputText,
        string userName)
    {
        var user = await context.UserProfiles.SingleOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            throw new BadRequestException($"Пользователь {userName} не найден");
        }
        
        var summarizationExisting = await context.Summarizations
            .AsNoTracking()
            .Where(s => s.InputText == inputText)
            .SingleOrDefaultAsync();

        if (summarizationExisting != null)
        {
            return new SummarizationSimpleResult
            {
                InputText = summarizationExisting.InputText ?? "",
                TopicText = summarizationExisting.TopicText ?? "",
                OutputSummarizedText = summarizationExisting.OutputSummarizedText ?? "",
                CreatedAt = summarizationExisting.CreatedAt,
            };
        }
        
        var result = await httpClient.GetSummarizationFromApi(inputText);

        var newSum = new Summarization(new CreateSimpleSummarizationParams
        {
            InputText = inputText,
            TopicText = result.Topic,
            OutputSummarizedText = result.Summarized,
            CreatedBy = user,
        });
        
        context.Summarizations.Add(newSum);
        
        await context.SaveChangesAsync();
        
        return new SummarizationSimpleResult
        {
            InputText = newSum.InputText ?? "",
            TopicText = newSum.TopicText ?? "",
            OutputSummarizedText = newSum.OutputSummarizedText ?? "",
            CreatedAt = newSum.CreatedAt,
        };;
    }
}
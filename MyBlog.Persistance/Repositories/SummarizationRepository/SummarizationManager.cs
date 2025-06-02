using System.Globalization;
using System.Reflection;
using ExcelToEnumerable;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Entities.Summarizations;
using MyBlog.Domain.Entities.Summarizations.Parameters;
using MyBlog.Domain.Exceptions;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.ExternalFeatures.SummarizationModelApis;
using MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;
using NPOI.XSSF.UserModel;

namespace MyBlog.Persistance.Repositories.SummarizationRepository;

public class SummarizationManager
{
    private readonly ApplicationDbContext context;
    private readonly SummarizationModelHttpClient httpClient;

    static readonly string SlnPath = Directory.GetParent(Directory.GetCurrentDirectory())!.ToString();
    static string _sumFolderPath = Path.Combine(SlnPath, "MyBlog.Web", "wwwroot", "summarizationFiles");

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
        };
        ;
    }

    public void CreateSummarizationFromFile(
        IFormFile file,
        string userName)
    {
        var xlsxInputPath = Path.Combine(_sumFolderPath, file.FileName);
        using (var inputFileStream = new FileStream(xlsxInputPath, FileMode.Create, FileAccess.Write))
        {
            file.CopyTo(inputFileStream);
        }
        
        var inputFileRows =
            xlsxInputPath.ExcelToEnumerable<TextDto>()
                .ToList();

        using var workbook = new XSSFWorkbook();

        var sheet = workbook.CreateSheet("Summarization");

        int rowIndex = 0;

        var row = sheet.CreateRow(rowIndex++);
        int cellIndex = 0;
        row.CreateCell(cellIndex++).SetCellValue("Text");
        row.CreateCell(cellIndex).SetCellValue("Summarization");

        foreach (var inputText in inputFileRows)
        {
            row = sheet.CreateRow(rowIndex++);
            cellIndex = 0;
            row.CreateCell(cellIndex++).SetCellValue(inputText.Text);
            row.CreateCell(cellIndex).SetCellValue("sum");
        }

        var filename = $"{userName}_sum_output_{DateTime.Now:yymmssfff}.xlsx";

        var xlsxOutputPath = Path.Combine(_sumFolderPath, filename);

        using var fileStream = new FileStream(xlsxOutputPath, FileMode.Create, FileAccess.Write);
        workbook.Write(fileStream);
    }
}
using System.Globalization;
using System.Reflection;
using ExcelToEnumerable;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Entities;
using MyBlog.Domain.Entities.Summarizations;
using MyBlog.Domain.Entities.Summarizations.Parameters;
using MyBlog.Domain.Exceptions;
using MyBlog.Persistance.Database;
using MyBlog.Persistance.ExternalFeatures.SummarizationModelApis;
using MyBlog.Persistance.Repositories.SummarizationRepository.Dtos;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;

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

    public async Task<ProjectSubsDto> GetProjectSubs(int postId)
    {
        var posts = await context.UserSubs
            .Where(s => s.PostId == postId)
            .ToListAsync();

        return new ProjectSubsDto
        {
            Usernames = posts.Select(s => s.Username).ToList()
        };
    }

    public async Task AddProjectSubs(
        int postId,
        string username)
    {
        context.UserSubs.Add(new UserSub(username, postId));
        await context.SaveChangesAsync();
    }


    public async Task<SummarizationResultDto> GetSummarizationAsync(int sumId)
    {
        var sum = await context.Summarizations
            .Include(s => s.Author)
            .SingleAsync(x => x.Id == sumId);

        return new SummarizationResultDto
        {
            SumId = sum.Id,
            PostId = sum.PostId,
            IsFile = sum.InputFilePath != null,
            CreatedAt = sum.CreatedAt.ToShortDateString(),
            CreatedBy = sum.Author.UserName,
            InputText = sum.InputText,
            OutputText = sum.OutputSummarizedText,
            InputFileName = sum.InputFileName,
            OutputFileName = $"Суммаризация {sum.InputFileName}"
        };
    }

    public async Task<SummarizationSimpleResult> DoSimpleSummarization(
        int postId,
        string inputText,
        string userName)
    {
        var user = await context.UserProfiles.SingleOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            throw new BadRequestException($"Пользователь {userName} не найден");
        }

        var post = context.Posts.Single(p => p.Id == postId);

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
            PostId = postId,
            Post = post,
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
        int postId,
        IFormFile file,
        string userName)
    {
        var fileExtension = Path.GetExtension(file.FileName);

        var fileInputPath =
            Path.Combine(_sumFolderPath, $"{userName}_sum_input_{DateTime.Now:yymmssfff}{fileExtension}");
        var filename = $"{userName}_sum_output_{DateTime.Now:yymmssfff}{fileExtension}";
        var fileOutputPath = Path.Combine(_sumFolderPath, filename);

        var user = context.UserProfiles.Single(u => u.UserName == userName);

        var post = context.Posts.Single(p => p.Id == postId);

        var newSummarization = new Summarization(new CreateFileSummarizationParams
        {
            InputFileName = file.FileName,
            InputFilePath = fileInputPath,
            OutputSummarizedFilePath = fileOutputPath,
            CreatedBy = user,
            PostId = postId,
            Post = post,
        });

        context.Summarizations.Add(newSummarization);
        context.SaveChanges();

        if (fileExtension == ".xlsx")
        {
            DoExcelSummarization(file, fileInputPath, fileOutputPath);
        }

        if (fileExtension == ".docx")
        {
            DoWordSummarization(file, fileInputPath, fileOutputPath);
        }
    }

    private static void DoWordSummarization(
        IFormFile file,
        string fileInputPath,
        string fileOutputPath)
    {
        using (var inputFileStream = new FileStream(fileInputPath, FileMode.Create, FileAccess.Write))
        {
            file.CopyTo(inputFileStream);
        }

        using (var fs = new FileStream(fileOutputPath, FileMode.Create, FileAccess.Write))
        {
            XWPFDocument doc = new XWPFDocument();
            var p0 = doc.CreateParagraph();
            p0.Alignment = ParagraphAlignment.CENTER;
            XWPFRun r0 = p0.CreateRun();
            r0.FontFamily = "microsoft yahei";
            r0.FontSize = 18;
            r0.IsBold = true;
            r0.SetText("This is title");

            var p1 = doc.CreateParagraph();
            p1.Alignment = ParagraphAlignment.LEFT;
            p1.IndentationFirstLine = 500;
            XWPFRun r1 = p1.CreateRun();
            r1.FontFamily = "·ÂËÎ";
            r1.FontSize = 12;
            r1.IsBold = true;
            r1.SetText("This is content, content content content content content content content content content");

            doc.Write(fs);
        }
    }

    private static void DoExcelSummarization(
        IFormFile file,
        string fileInputPath,
        string fileOutputPath)
    {
        using (var inputFileStream = new FileStream(fileInputPath, FileMode.Create, FileAccess.Write))
        {
            file.CopyTo(inputFileStream);
        }

        var inputFileRows =
            fileInputPath.ExcelToEnumerable<TextDto>()
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
            //TODO! UNCOM AFTER TEST
            // var result = httpClient.GetSummarizationFromApi(inputText.Text).GetAwaiter().GetResult();

            row = sheet.CreateRow(rowIndex++);
            cellIndex = 0;
            row.CreateCell(cellIndex++).SetCellValue(inputText.Text);
            //TODO! UNCOM AFTER TEST
            // row.CreateCell(cellIndex).SetCellValue(result.Summarized);
            row.CreateCell(cellIndex).SetCellValue("sum");
        }

        using var fileStream = new FileStream(fileOutputPath, FileMode.Create, FileAccess.Write);
        workbook.Write(fileStream);
    }

    public FileDto DownloadSummarizationInput(
        int sumId)
    {
        var summarization = context.Summarizations
            .Single(p => p.Id == sumId);

        if (summarization.InputFilePath == null)
        {
            throw new BadRequestException("Нет файла суммаризации");
        }

        var file = new FileDto
        {
            Bytes = File.ReadAllBytes(summarization.InputFilePath!),
            FileName = summarization.InputFilePath!.Split("_").Last(),
        };

        return file;
    }

    public FileDto DownloadSummarizationOutput(int sumId)
    {
        var summarization = context.Summarizations
            .Single(p => p.Id == sumId);

        if (summarization.InputFilePath == null)
        {
            throw new BadRequestException("Нет файла суммаризации");
        }

        var file = new FileDto
        {
            Bytes = File.ReadAllBytes(summarization.OutputSummarizedFilePath!),
            FileName = "Суммаризация " + summarization.InputFilePath!.Split("_").Last(),
        };

        return file;
    }
}